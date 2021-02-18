using DivineMonad.Data;
using DivineMonad.Models;
using DivineMonad.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DivineMonad.Controllers
{
    [Authorize(Policy = "user")]
    public class CharactersController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IDbContextHelper _contextHelper;
        private readonly IWebHostEnvironment _hostingEnv;

        public CharactersController(
            ApplicationDbContext context,
            IDbContextHelper contextHelper,
            IWebHostEnvironment hostingEnv)
        {
            _context = context;
            _contextHelper = contextHelper;
            _hostingEnv = hostingEnv;
        }


        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var characters = _context.Characters
                .Where(c => c.UserId == userId).Include(c => c.CBStats);

            ViewData["userId"] = userId;
            ViewData["userName"] = userName;

            return View(await characters.ToListAsync());
        }


        public IActionResult Create()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!(userId is null))
            {
                Character newCharacter = new Character { UserId = userId };
                return View(newCharacter);
            }
            else return RedirectToAction("Index", "Characters");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,AvatarImage,UserId")] Character character)
        {
            if (ModelState.IsValid)
            {
                character.CBStats = new CharacterBaseStats("new");
                character.GStats = new GameStats("new");

                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                string a = _hostingEnv.WebRootPath;

                string characterDataPath = Path.Combine(a, "data\\" + userId.ToString() + "\\" + character.Name);

                if (!Directory.Exists(characterDataPath))
                {
                    DirectoryInfo di = Directory.CreateDirectory(characterDataPath + "\\raports");
                }

                if (character.AvatarImage != null)
                {
                    string AvatarExtension = Path.GetExtension(character.AvatarImage.FileName);
                    var filePath = Path.Combine(characterDataPath, "avatar" + AvatarExtension);

                    using (var fileSteam = new FileStream(filePath, FileMode.Create))
                    {
                        await character.AvatarImage.CopyToAsync(fileSteam);
                    }

                    character.AvatarUrl = "~\\" + filePath.Split("wwwroot\\")[1];
                    character.AvatarUrl = character.AvatarUrl.Replace("\\", "/");
                }
                else
                {
                    var filePath = Path.Combine(a, "images\\avatars\\avatar_default.png");
                    character.AvatarUrl = "~\\" + filePath.Split("wwwroot\\")[1];
                    character.AvatarUrl = character.AvatarUrl.Replace("\\", "/");
                }

                _context.Add(character);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(character);
        }


        [AcceptVerbs("GET", "POST")]
        public IActionResult IsNameUnique(string name, int id)
        {
            bool isEditMode = Request.Headers["Referer"].ToString().Contains("Characters/Edit");
            bool isManageMode = Request.Headers["Referer"].ToString().Contains("Manage");

            if (isEditMode || isManageMode)
            {
                if (_context.Characters.FirstOrDefault(c => c.ID == id).Name.Equals(name))
                    return Json(true);
                if (_context.Characters.Any(c => c.Name == name))
                    return Json($"Character name already exists.");
            }
            else
            {
                if (_context.Characters.Any(c => c.Name == name))
                    return Json($"Character name already exists.");
            }

            return Json(true);
        }
    }
}
