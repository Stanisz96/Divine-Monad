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
    public class ItemsStatsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ItemsStatsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Items"] = new SelectList(
                _context.Items, "StatisticsId", "Name");
            ViewData["ItemsId"] = new SelectList(
                _context.Items, "StatisticsId", "ID");

            var rarities = await _context.Items
                .Include(i => i.Rarity)
                .Select(i => new { i.Rarity.Name, i.StatisticsId })
                .ToListAsync();

            ViewData["Rarities"] = new SelectList(
                rarities, "StatisticsId", "Name");

            return View(await _context.ItemsStats.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var itemStats = await _context.ItemsStats
                .FirstOrDefaultAsync(m => m.ID == id);

            if (itemStats == null) return NotFound();

            ViewData["ItemName"] = _context.Items
                .FirstOrDefault(c => c.StatisticsId == id).Name;
            ViewData["ItemId"] = _context.Items
                .FirstOrDefault(c => c.StatisticsId == id).ID;

            return View(itemStats);
        }

        public IActionResult Create() => 
            return View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("ID,Stamina,Strength,Agility,Dexterity," +
            "Luck,HitPoints,AttackMin,AttackMax,Armor," +
            "Block,Dodge,Speed,CritChance,Accuracy")]
                ItemStats itemStats)
        {
            if (ModelState.IsValid)
            {
                _context.Add(itemStats);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(itemStats);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var itemStats = await _context
                .ItemsStats.FindAsync(id);

            if (itemStats == null) return NotFound();

            return View(itemStats);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("ID,Stamina,Strength,Agility,Dexterity," +
            "Luck,HitPoints,AttackMin,AttackMax,Armor," +
            "Block,Dodge,Speed,CritChance,Accuracy")] 
                ItemStats itemStats)
        {
            if (id != itemStats.ID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(itemStats);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemStatsExists(itemStats.ID)) return NotFound();
                    else throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(itemStats);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var itemStats = await _context.ItemsStats
                .FirstOrDefaultAsync(m => m.ID == id);

            if (itemStats == null) return NotFound();

            return View(itemStats);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var itemStats = await _context.ItemsStats.FindAsync(id);

            _context.ItemsStats.Remove(itemStats);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool ItemStatsExists(int id) => 
            _context.ItemsStats.Any(e => e.ID == id);
    }
}
