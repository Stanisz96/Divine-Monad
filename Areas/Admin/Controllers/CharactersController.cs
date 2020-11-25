﻿using System;
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
    public class CharactersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CharactersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Characters
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Characters.Include(c => c.CBStats).Include(c => c.GStats);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/Characters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var character = await _context.Characters
                .Include(c => c.CBStats)
                .Include(c => c.GStats)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (character == null)
            {
                return NotFound();
            }

            return View(character);
        }

        // GET: Admin/Characters/Create
        public IActionResult Create()
        {
            ViewData["CBStatsId"] = new SelectList(_context.CharactersBaseStats, "ID", "ID");
            ViewData["GStatsId"] = new SelectList(_context.CharactersGameStats, "ID", "ID");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName");

            return View();
        }

        // POST: Admin/Characters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,UserId,CBStatsId,GStatsId")] Character character)
        {
            if (ModelState.IsValid)
            {
                _context.Add(character);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CBStatsId"] = new SelectList(_context.CharactersBaseStats, "ID", "ID", character.CBStatsId);
            ViewData["GStatsId"] = new SelectList(_context.CharactersGameStats, "ID", "ID", character.GStatsId);
            return View(character);
        }

        // GET: Admin/Characters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var character = await _context.Characters.FindAsync(id);
            if (character == null)
            {
                return NotFound();
            }
            ViewData["CBStatsId"] = new SelectList(_context.CharactersBaseStats, "ID", "ID", character.CBStatsId);
            ViewData["GStatsId"] = new SelectList(_context.CharactersGameStats, "ID", "ID", character.GStatsId);
            return View(character);
        }

        // POST: Admin/Characters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,UserId,CBStatsId,GStatsId")] Character character)
        {
            if (id != character.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(character);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CharacterExists(character.ID))
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
            ViewData["CBStatsId"] = new SelectList(_context.CharactersBaseStats, "ID", "ID", character.CBStatsId);
            ViewData["GStatsId"] = new SelectList(_context.CharactersGameStats, "ID", "ID", character.GStatsId);
            return View(character);
        }

        // GET: Admin/Characters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var character = await _context.Characters
                .Include(c => c.CBStats)
                .Include(c => c.GStats)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (character == null)
            {
                return NotFound();
            }

            return View(character);
        }

        // POST: Admin/Characters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var character = await _context.Characters.FindAsync(id);
            _context.Characters.Remove(character);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CharacterExists(int id)
        {
            return _context.Characters.Any(e => e.ID == id);
        }
    }
}
