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
    public class ItemsCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ItemsCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index() =>
            View(await _context.ItemCategories.ToListAsync());

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var itemCategory = await _context.ItemCategories
                .FirstOrDefaultAsync(m => m.ID == id);

            if (itemCategory == null) return NotFound();

            return View(itemCategory);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("ID,Name,Description,Armor,Arrow," +
            "Boots,Bow,Gloves,Helmet,Weapon1H,Weapon2H,Shield")] 
                ItemCategory itemCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(itemCategory);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(itemCategory);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var itemCategory = await _context
                .ItemCategories.FindAsync(id);

            if (itemCategory == null) return NotFound();

            return View(itemCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("ID,Name,Description,Armor,Arrow,Boots," +
            "Bow,Gloves,Helmet,Weapon1H,Weapon2H,Shield")] 
                ItemCategory itemCategory)
        {
            if (id != itemCategory.ID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(itemCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemCategoryExists(itemCategory.ID)) return NotFound();
                    else throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(itemCategory);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var itemCategory = await _context.ItemCategories
                .FirstOrDefaultAsync(m => m.ID == id);

            if (itemCategory == null) return NotFound();

            return View(itemCategory);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var itemCategory = await _context.ItemCategories.FindAsync(id);

            _context.ItemCategories.Remove(itemCategory);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool ItemCategoryExists(int id) =>
            _context.ItemCategories.Any(e => e.ID == id);
    }
}
