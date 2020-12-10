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
    public class ItemsStatsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ItemsStatsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/ItemsStats
        public async Task<IActionResult> Index()
        {
            ViewData["Items"] = new SelectList(_context.Items, "StatisticsId", "Name");
            ViewData["ItemsId"] = new SelectList(_context.Items, "StatisticsId", "ID");

            return View(await _context.ItemsStats.ToListAsync());
        }

        // GET: Admin/ItemsStats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemStats = await _context.ItemsStats
                .FirstOrDefaultAsync(m => m.ID == id);
            if (itemStats == null)
            {
                return NotFound();
            }

            ViewData["ItemName"] = _context.Items.FirstOrDefault(c => c.StatisticsId == id).Name;
            ViewData["ItemId"] = _context.Items.FirstOrDefault(c => c.StatisticsId == id).ID;

            return View(itemStats);
        }

        // GET: Admin/ItemsStats/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/ItemsStats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Stamina,Strength,Agility,Dexterity,Luck,HitPoints,AttackMin,AttackMax,Armor,Block,Dodge,Speed,CritChance,Accuracy")] ItemStats itemStats)
        {
            if (ModelState.IsValid)
            {
                _context.Add(itemStats);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(itemStats);
        }

        // GET: Admin/ItemsStats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemStats = await _context.ItemsStats.FindAsync(id);
            if (itemStats == null)
            {
                return NotFound();
            }
            return View(itemStats);
        }

        // POST: Admin/ItemsStats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Stamina,Strength,Agility,Dexterity,Luck,HitPoints,AttackMin,AttackMax,Armor,Block,Dodge,Speed,CritChance,Accuracy")] ItemStats itemStats)
        {
            if (id != itemStats.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(itemStats);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemStatsExists(itemStats.ID))
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
            return View(itemStats);
        }

        // GET: Admin/ItemsStats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemStats = await _context.ItemsStats
                .FirstOrDefaultAsync(m => m.ID == id);
            if (itemStats == null)
            {
                return NotFound();
            }

            return View(itemStats);
        }

        // POST: Admin/ItemsStats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var itemStats = await _context.ItemsStats.FindAsync(id);
            _context.ItemsStats.Remove(itemStats);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemStatsExists(int id)
        {
            return _context.ItemsStats.Any(e => e.ID == id);
        }
    }
}
