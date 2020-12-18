﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DivineMonad.Data;
using DivineMonad.Models;

namespace DivineMonad.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MonstersStatsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MonstersStatsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/MonstersStats
        public async Task<IActionResult> Index()
        {
            ViewData["Monsters"] = new SelectList(_context.Monsters, "MonsterStatsId", "Name");
            ViewData["MonstersId"] = new SelectList(_context.Monsters, "MonsterStatsId", "ID");

            return View(await _context.MonstersStats.ToListAsync());
        }

        // GET: Admin/MonstersStats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var monsterStats = await _context.MonstersStats
                .FirstOrDefaultAsync(m => m.ID == id);
            if (monsterStats == null)
            {
                return NotFound();
            }

            ViewData["MonsterName"] = _context.Monsters.FirstOrDefault(c => c.MonsterStatsId == id).Name;
            ViewData["MonsterId"] = _context.Monsters.FirstOrDefault(c => c.MonsterStatsId == id).ID;

            return View(monsterStats);
        }

        // GET: Admin/MonstersStats/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/MonstersStats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Stamina,Strength,Agility,Dexterity,Luck,HitPoints,AttackMin,Attack,AttackMax,Armor,Block,Dodge,Speed,CritChance,Accuracy")] MonsterStats monsterStats)
        {
            if (ModelState.IsValid)
            {
                _context.Add(monsterStats);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(monsterStats);
        }

        // GET: Admin/MonstersStats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var monsterStats = await _context.MonstersStats.FindAsync(id);
            if (monsterStats == null)
            {
                return NotFound();
            }
            return View(monsterStats);
        }

        // POST: Admin/MonstersStats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Stamina,Strength,Agility,Dexterity,Luck,HitPoints,AttackMin,Attack,AttackMax,Armor,Block,Dodge,Speed,CritChance,Accuracy")] MonsterStats monsterStats)
        {
            if (id != monsterStats.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(monsterStats);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MonsterStatsExists(monsterStats.ID))
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
            return View(monsterStats);
        }

        // GET: Admin/MonstersStats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var monsterStats = await _context.MonstersStats
                .FirstOrDefaultAsync(m => m.ID == id);
            if (monsterStats == null)
            {
                return NotFound();
            }

            return View(monsterStats);
        }

        // POST: Admin/MonstersStats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var monsterStats = await _context.MonstersStats.FindAsync(id);
            _context.MonstersStats.Remove(monsterStats);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MonsterStatsExists(int id)
        {
            return _context.MonstersStats.Any(e => e.ID == id);
        }
    }
}