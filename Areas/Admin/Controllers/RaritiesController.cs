using System;
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
    public class RaritiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RaritiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Rarities
        public async Task<IActionResult> Index()
        {
            return View(await _context.Rarity.ToListAsync());
        }

        // GET: Admin/Rarities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rarity = await _context.Rarity
                .FirstOrDefaultAsync(m => m.ID == id);
            if (rarity == null)
            {
                return NotFound();
            }

            return View(rarity);
        }

        // GET: Admin/Rarities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Rarities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Chance")] Rarity rarity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rarity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rarity);
        }

        // GET: Admin/Rarities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rarity = await _context.Rarity.FindAsync(id);
            if (rarity == null)
            {
                return NotFound();
            }
            return View(rarity);
        }

        // POST: Admin/Rarities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Chance")] Rarity rarity)
        {
            if (id != rarity.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rarity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RarityExists(rarity.ID))
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
            return View(rarity);
        }

        // GET: Admin/Rarities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rarity = await _context.Rarity
                .FirstOrDefaultAsync(m => m.ID == id);
            if (rarity == null)
            {
                return NotFound();
            }

            return View(rarity);
        }

        // POST: Admin/Rarities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rarity = await _context.Rarity.FindAsync(id);
            _context.Rarity.Remove(rarity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RarityExists(int id)
        {
            return _context.Rarity.Any(e => e.ID == id);
        }
    }
}
