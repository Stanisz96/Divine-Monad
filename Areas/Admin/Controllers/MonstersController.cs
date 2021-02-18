﻿using DivineMonad.Data;
using DivineMonad.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DivineMonad.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class MonstersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MonstersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context
                .Monsters.Include(m => m.MonsterStats);

            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var monster = await _context.Monsters
                .Include(m => m.MonsterStats)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (monster == null) return NotFound();

            return View(monster);
        }

        public IActionResult Create()
        {
            ViewData["MonsterStatsId"] = new SelectList(
                _context.MonstersStats, "ID", "ID");

            var statisticsId = new SelectList(
                _context.MonstersStats, "ID", "ID");

            ViewData["StatisticsId"] = statisticsId;
            ViewData["DefaultStatsId"] = statisticsId.FirstOrDefault().Value;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("ID,Name,Description,ImageUrl," +
            "Level,Gold,Experience,MonsterStatsId")] Monster monster)
        {
            if (ModelState.IsValid)
            {
                _context.Add(monster);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["MonsterStatsId"] = new SelectList(
                _context.MonstersStats, "ID", "ID", monster.MonsterStatsId);
            
            return View(monster);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var monster = await _context
                .Monsters.FindAsync(id);

            if (monster == null) return NotFound();

            ViewData["MonsterStatsId"] = new SelectList(
                _context.MonstersStats, "ID", "ID", monster.MonsterStatsId);

            var statisticsId = new SelectList(_context.MonstersStats, "ID", "ID");

            ViewData["StatisticsId"] = statisticsId;
            ViewData["DefaultStatsId"] = statisticsId.FirstOrDefault().Value;

            return View(monster);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("ID,Name,Description,ImageUrl,Level," +
            "Gold,Experience,MonsterStatsId")] Monster monster)
        {
            if (id != monster.ID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(monster);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MonsterExists(monster.ID)) return NotFound();
                    else throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["MonsterStatsId"] = new SelectList(
                _context.MonstersStats, "ID", "ID", monster.MonsterStatsId);

            return View(monster);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var monster = await _context.Monsters
                .Include(m => m.MonsterStats)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (monster == null) return NotFound();

            return View(monster);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var monster = await _context.Monsters.FindAsync(id);

            _context.Monsters.Remove(monster);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult ReloadViewComponent(int newId) =>
            ViewComponent("ShowExternalProps", new { id = newId, type = "MonsterStatistics" });


        private bool MonsterExists(int id) =>
            _context.Monsters.Any(e => e.ID == id);
    }
}
