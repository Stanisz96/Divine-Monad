using DivineMonad.Data;
using DivineMonad.Models;
using DivineMonad.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DivineMonad.Controllers
{
    [Authorize(Policy = "user")]
    public class ManageController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IDbContextHelper _contextHelper;
        private readonly IWebHostEnvironment _hostingEnv;

        public ManageController(ApplicationDbContext context, IDbContextHelper contextHelper, IWebHostEnvironment hostingEnv)
        {
            _context = context;
            _contextHelper = contextHelper;
            _hostingEnv = hostingEnv;
        }

        public async Task<IActionResult> Index(int cId)
        {
            Character character = await _contextHelper.GetCharacter(cId, User, _context);

            if (!(character is null))
            {
                return View(character);
            }
            else return RedirectToAction("Index", "Characters");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("ID,Name,AvatarImage,UserId")] Character character)
        {
            var updateCharacter = await _contextHelper.GetCharacter(character.ID, User, _context);
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (ModelState.IsValid)
            {
                if (!(updateCharacter is null))
                {
                    string a = _hostingEnv.WebRootPath;

                    if (updateCharacter.Name != character.Name)
                    {
                        Directory.Move(Path.Combine(a, "data", userId, updateCharacter.Name), Path.Combine(a, "data", userId, character.Name));
                        if (!updateCharacter.AvatarUrl.Contains("avatar_default"))
                            updateCharacter.AvatarUrl = updateCharacter.AvatarUrl.Replace(updateCharacter.Name, character.Name);
                    }
                    string AvatarPath = Path.Combine(a, updateCharacter.AvatarUrl.Replace("/", "\\").Replace("~\\", ""));

                    updateCharacter.Name = character.Name;
                    updateCharacter.AvatarImage = character.AvatarImage;


                    if (updateCharacter.AvatarImage != null)
                    {
                        if (AvatarPath.Contains("avatar_default"))
                        {
                            AvatarPath = AvatarPath.Replace("images\\avatars\\avatar_default.png", "data\\" + userId + "\\" +
                                updateCharacter.Name + "\\avatar.png");
                            updateCharacter.AvatarUrl = "~" + AvatarPath.Replace("\\", "/").Split("wwwroot")[1];
                        }

                        using (var fileSteam = new FileStream(AvatarPath, FileMode.Create))
                        {
                            await updateCharacter.AvatarImage.CopyToAsync(fileSteam);
                        }
                    }

                    _context.Update(updateCharacter);

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

            }
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromForm, Bind("ID")] int ID)
        {
            var character = await _contextHelper.GetCharacter(ID, User, _context);
            if (!(character is null))
            {
                _context.Remove(character);
                _context.Remove(character.GStats);
                _context.Remove(character.CBStats);
                _context.RemoveRange(_context.CharactersItems.Where(i => i.CharacterId == character.ID));


                // remove files
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                string a = _hostingEnv.WebRootPath;
                string characterPath = Path.Combine(a, "data\\" + userId + "\\" + character.Name);
                Directory.Delete(characterPath, true);

                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Characters");
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
