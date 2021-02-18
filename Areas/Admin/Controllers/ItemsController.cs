using DivineMonad.Data;
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
    public class ItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Items
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Items.Include(i => i.Category)
                .Include(i => i.Rarity).Include(i => i.Statistics);

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.Category)
                .Include(i => i.Rarity)
                .Include(i => i.Statistics)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Admin/Items/Create
        public IActionResult Create()
        {
            ViewData["Category"] = new SelectList(_context.ItemCategories, "ID", "Name");
            ViewData["RarityId"] = new SelectList(_context.Rarity, "ID", "Name");
            var statisticsId = new SelectList(_context.ItemsStats, "ID", "ID");
            ViewData["StatisticsId"] = statisticsId;
            ViewData["DefaultStatsId"] = statisticsId.FirstOrDefault().Value;

            return View();
        }

        // POST: Admin/Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Description,ImageUrl,Price,Level,CategoryId,StatisticsId,RarityId")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.ItemCategories, "ID", "ID", item.CategoryId);
            ViewData["RarityId"] = new SelectList(_context.Rarity, "ID", "ID", item.RarityId);
            ViewData["StatisticsId"] = new SelectList(_context.ItemsStats, "ID", "ID", item.StatisticsId);
            return View(item);
        }

        // GET: Admin/Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.ItemCategories, "ID", "Name", item.CategoryId);
            ViewData["RarityId"] = new SelectList(_context.Rarity, "ID", "Name", item.RarityId);
            ViewData["StatisticsId"] = new SelectList(_context.ItemsStats, "ID", "ID", item.StatisticsId);
            return View(item);
        }

        // POST: Admin/Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Description,ImageUrl,Price,Level,CategoryId,StatisticsId,RarityId")] Item item)
        {
            if (id != item.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.ID))
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
            ViewData["CategoryId"] = new SelectList(_context.ItemCategories, "ID", "Name", item.CategoryId);
            ViewData["RarityId"] = new SelectList(_context.Rarity, "ID", "Name", item.RarityId);
            ViewData["StatisticsId"] = new SelectList(_context.ItemsStats, "ID", "ID", item.StatisticsId);
            return View(item);
        }

        // GET: Admin/Items/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.Category)
                .Include(i => i.Rarity)
                .Include(i => i.Statistics)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Admin/Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Items.FindAsync(id);
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ReloadViewComponent(int newId)
        {
            return ViewComponent("ShowExternalProps", new { id = newId, type = "ItemStatistics" });
        }



        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.ID == id);
        }
    }
}
