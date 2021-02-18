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
    public class MarketsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MarketsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index() => 
            View(await _context.Markets.ToListAsync());

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var market = await _context.Markets
                .FirstOrDefaultAsync(m => m.ID == id);

            if (market == null) return NotFound();

            return View(market);
        }

        public IActionResult Create() => View();


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,LevelMin,LevelMax,ItemId")] Market market)
        {
            if (ModelState.IsValid)
            {
                _context.Add(market);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(market);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var market = await _context
                .Markets.FindAsync(id);

            if (market == null) return NotFound();

            return View(market);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("ID,LevelMin,LevelMax,ItemId")] Market market)
        {
            if (id != market.ID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(market);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MarketExists(market.ID)) return NotFound();
                    else throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(market);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var market = await _context.Markets
                .FirstOrDefaultAsync(m => m.ID == id);

            if (market == null) return NotFound();

            return View(market);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var market = await _context.Markets.FindAsync(id);

            _context.Markets.Remove(market);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool MarketExists(int id) =>
            _context.Markets.Any(e => e.ID == id);
    }
}
