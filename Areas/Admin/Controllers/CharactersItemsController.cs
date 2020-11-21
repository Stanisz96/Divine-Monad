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
    public class CharactersItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CharactersItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/CharactersItems
        public async Task<IActionResult> Index()
        {
            return View(await _context.CharactersItems.ToListAsync());
        }

        // GET: Admin/CharactersItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var characterItems = await _context.CharactersItems
                .FirstOrDefaultAsync(m => m.ID == id);
            if (characterItems == null)
            {
                return NotFound();
            }

            return View(characterItems);
        }

        // GET: Admin/CharactersItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/CharactersItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,CharacterId,ItemId,IsEquipped")] CharacterItems characterItems)
        {
            if (ModelState.IsValid)
            {
                _context.Add(characterItems);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(characterItems);
        }

        // GET: Admin/CharactersItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var characterItems = await _context.CharactersItems.FindAsync(id);
            if (characterItems == null)
            {
                return NotFound();
            }
            return View(characterItems);
        }

        // POST: Admin/CharactersItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,CharacterId,ItemId,IsEquipped")] CharacterItems characterItems)
        {
            if (id != characterItems.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(characterItems);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CharacterItemsExists(characterItems.ID))
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
            return View(characterItems);
        }

        // GET: Admin/CharactersItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var characterItems = await _context.CharactersItems
                .FirstOrDefaultAsync(m => m.ID == id);
            if (characterItems == null)
            {
                return NotFound();
            }

            return View(characterItems);
        }

        // POST: Admin/CharactersItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var characterItems = await _context.CharactersItems.FindAsync(id);
            _context.CharactersItems.Remove(characterItems);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CharacterItemsExists(int id)
        {
            return _context.CharactersItems.Any(e => e.ID == id);
        }
    }
}
