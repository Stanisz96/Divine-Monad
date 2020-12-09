using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DivineMonad.Data;
using DivineMonad.Models;
using Microsoft.AspNetCore.Authorization;

namespace DivineMonad.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CharactersGameStatsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CharactersGameStatsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/CharactersGameStats
        public async Task<IActionResult> Index()
        {
            ViewData["Characters"] = new SelectList(_context.Characters, "GStatsId", "Name");

            return View(await _context.CharactersGameStats.ToListAsync());
        }

        // GET: Admin/CharactersGameStats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameStats = await _context.CharactersGameStats
                .FirstOrDefaultAsync(m => m.ID == id);
            if (gameStats == null)
            {
                return NotFound();
            }

            ViewData["CharacterName"] = _context.Characters.FirstOrDefault(c => c.GStatsId == id).Name;

            return View(gameStats);
        }

        // GET: Admin/CharactersGameStats/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/CharactersGameStats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,MonsterKills,CollectedGold,DeathsNumber,LostFights,WinFights,DrawFights,LootedNormal,LootedUnique,LootedHeroic,LootedLegendary")] GameStats gameStats)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gameStats);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gameStats);
        }

        // GET: Admin/CharactersGameStats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameStats = await _context.CharactersGameStats.FindAsync(id);
            if (gameStats == null)
            {
                return NotFound();
            }
            return View(gameStats);
        }

        // POST: Admin/CharactersGameStats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,MonsterKills,CollectedGold,DeathsNumber,LostFights,WinFights,DrawFights,LootedNormal,LootedUnique,LootedHeroic,LootedLegendary")] GameStats gameStats)
        {
            if (id != gameStats.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gameStats);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameStatsExists(gameStats.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(gameStats);
        }

        // GET: Admin/CharactersGameStats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameStats = await _context.CharactersGameStats
                .FirstOrDefaultAsync(m => m.ID == id);
            if (gameStats == null)
            {
                return NotFound();
            }

            return View(gameStats);
        }

        // POST: Admin/CharactersGameStats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gameStats = await _context.CharactersGameStats.FindAsync(id);
            _context.CharactersGameStats.Remove(gameStats);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameStatsExists(int id)
        {
            return _context.CharactersGameStats.Any(e => e.ID == id);
        }
    }
}
