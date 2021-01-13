using DivineMonad.Data;
using DivineMonad.Models;
using DivineMonad.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public CharactersController(ApplicationDbContext context, IDbContextHelper contextHelper)
        {
            _context = context;
            _contextHelper = contextHelper;
        }

        
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var characters = _context.Characters.Where(c => c.UserId == userId).Include(c => c.CBStats);

            ViewData["userId"] = userId;
            ViewData["userName"] = userName;

            return View(await characters.ToListAsync());
        }


        public IActionResult Create()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(!(userId is null))
            {
                Character newCharacter = new Character { UserId = userId };
                return View(newCharacter);
            }
            else return RedirectToAction("Index", "Characters");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,UserId")] Character character)
        {
            if (ModelState.IsValid)
            {
                character.CBStats = new CharacterBaseStats("new");
                character.GStats = new GameStats("new");

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
