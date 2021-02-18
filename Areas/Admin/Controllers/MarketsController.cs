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

        // GET: Admin/Markets
        public async Task<IActionResult> Index()
        {
            return View(await _context.Markets.ToListAsync());
        }

        // GET: Admin/Markets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var market = await _context.Markets
                .FirstOrDefaultAsync(m => m.ID == id);
            if (market == null)
            {
                return NotFound();
            }

            return View(market);
        }

        // GET: Admin/Markets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Markets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Admin/Markets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var market = await _context.Markets.FindAsync(id);
            if (market == null)
            {
                return NotFound();
            }
            return View(market);
        }

        // POST: Admin/Markets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,LevelMin,LevelMax,ItemId")] Market market)
        {
            if (id != market.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(market);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MarketExists(market.ID))
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
            return View(market);
        }

        // GET: Admin/Markets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var market = await _context.Markets
                .FirstOrDefaultAsync(m => m.ID == id);
            if (market == null)
            {
                return NotFound();
            }

            return View(market);
        }

        // POST: Admin/Markets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var market = await _context.Markets.FindAsync(id);
            _context.Markets.Remove(market);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MarketExists(int id)
        {
            return _context.Markets.Any(e => e.ID == id);
        }
    }
}
