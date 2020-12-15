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

                if (userId.Equals(character.UserId)) return View();
                else return NotFound();
            }
            catch (Exception)
            {
                return NotFound();
            }
           
        }
    }
}
