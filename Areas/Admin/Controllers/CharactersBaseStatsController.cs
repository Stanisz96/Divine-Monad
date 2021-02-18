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
    public class CharactersBaseStatsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CharactersBaseStatsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Characters"] = new SelectList(_context.Characters, "CBStatsId", "Name");
            ViewData["CharactersId"] = new SelectList(_context.Characters, "CBStatsId", "ID");

            return View(await _context.CharactersBaseStats.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var characterBaseStats = await _context.CharactersBaseStats
                .FirstOrDefaultAsync(m => m.ID == id);

            if (characterBaseStats == null) return NotFound();

            ViewData["CharacterName"] = _context.Characters
                .FirstOrDefault(c => c.CBStatsId == id).Name;
            ViewData["CharacterId"] = _context.Characters
                .FirstOrDefault(c => c.CBStatsId == id).ID;

            return View(characterBaseStats);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("ID,Level,Experience,Gold,BpSlots,Stamina,Strength,Agility,Dexterity,Luck,StatsPoints")] 
                CharacterBaseStats characterBaseStats)
        {
            if (ModelState.IsValid)
            {
                _context.Add(characterBaseStats);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(characterBaseStats);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var characterBaseStats = await _context
                .CharactersBaseStats.FindAsync(id);

            if (characterBaseStats == null) return NotFound();

            return View(characterBaseStats);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("ID,Level,Experience,Gold,BpSlots,Stamina,Strength,Agility,Dexterity,Luck,StatsPoints")]
                CharacterBaseStats characterBaseStats)
        {
            if (id != characterBaseStats.ID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(characterBaseStats);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CharacterBaseStatsExists(characterBaseStats.ID)) return NotFound();
                    else throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(characterBaseStats);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var characterBaseStats = await _context.CharactersBaseStats
                .FirstOrDefaultAsync(m => m.ID == id);

            if (characterBaseStats == null) return NotFound();

            return View(characterBaseStats);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var characterBaseStats = await _context.CharactersBaseStats.FindAsync(id);
            _context.CharactersBaseStats.Remove(characterBaseStats);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CharacterBaseStatsExists(int id) => 
            _context.CharactersBaseStats.Any(e => e.ID == id);
    }
}
