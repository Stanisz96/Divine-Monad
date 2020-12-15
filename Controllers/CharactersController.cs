using DivineMonad.Data;
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

        public CharactersController(ApplicationDbContext context)
        {
            _context = context;
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

            return View();
        }
    }
}
