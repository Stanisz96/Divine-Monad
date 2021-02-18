using DivineMonad.Data;
using DivineMonad.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DivineMonad.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RaritiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RaritiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index() =>
            View(await _context.Rarity.ToListAsync());

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var rarity = await _context.Rarity
                .FirstOrDefaultAsync(m => m.ID == id);

            if (rarity == null) return NotFound();

            return View(rarity);
        }

        public IActionResult Create() => View();

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

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var rarity = await _context
                .Rarity.FindAsync(id);

            if (rarity == null) return NotFound();

            return View(rarity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("ID,Name,Chance")] Rarity rarity)
        {
            if (id != rarity.ID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rarity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RarityExists(rarity.ID)) return NotFound();
                    else throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(rarity);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var rarity = await _context.Rarity
                .FirstOrDefaultAsync(m => m.ID == id);

            if (rarity == null) return NotFound();

            return View(rarity);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rarity = await _context.Rarity.FindAsync(id);

            _context.Rarity.Remove(rarity);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool RarityExists(int id) =>
            _context.Rarity.Any(e => e.ID == id);
    }
}
