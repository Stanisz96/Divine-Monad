using DivineMonad.Data;
using DivineMonad.Models;
using Microsoft.AspNetCore.Authorization;
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
    public class GameController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GameController(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index(int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                Character character = await _context.Characters.Include(c => c.CBStats)
                                        .Include(c => c.GStats)
                                        .FirstOrDefaultAsync(m => m.ID == id);
                ViewData["id"] = id;

                if (userId.Equals(character.UserId)) return View(character);
                else return View("NotNice");
            }
            catch (Exception)
            {
                return NotFound();
            }
           
        }

        public IActionResult ReloadViewComponent(int _cId, int _bsId, string type)
        {
            if (type.Equals("game-character")) return ViewComponent("GameCharacter", new { cId = _cId, bsId = _bsId });
            else if (type.Equals("game-backpack")) return ViewComponent("GameBackpack");
            else if (type.Equals("game-battle")) return ViewComponent("GameBattle");
            else if (type.Equals("game-market")) return ViewComponent("GameMarket");
            else if (type.Equals("game-news")) return ViewComponent("GameNews");
            else return NotFound();
        }
    }
}
