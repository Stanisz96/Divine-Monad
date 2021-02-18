using DivineMonad.Data;
using DivineMonad.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DivineMonad.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class MonstersLootController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MonstersLootController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/MonstersLoot
        public async Task<IActionResult> Index()
        {
            var monstersLootList = await _context.MonstersLoot.ToListAsync();

            ViewData["Monsters"] = new SelectList(_context.Monsters, "ID", "Name");
            ViewData["Items"] = new SelectList(_context.Items, "ID", "Name");
            var rarities = await _context.Items.Include(i => i.Rarity).Select(i => new { i.Rarity.Name, i.ID }).ToListAsync();
            ViewData["Rarities"] = new SelectList(rarities, "ID", "Name");

            return View(monstersLootList);
        }

        // GET: Admin/MonstersLoot/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var monsterLoot = await _context.MonstersLoot
                .FirstOrDefaultAsync(m => m.ID == id);
            if (monsterLoot == null)
            {
                return NotFound();
            }

            return View(monsterLoot);
        }

        // GET: Admin/MonstersLoot/Create
        public async Task<IActionResult> Create()
        {
            ViewData["Monsters"] = new SelectList(_context.Monsters, "ID", "Name");
            //ViewData["Items"] = new SelectList(_context.Items, "ID", "Name");
            var items = await _context.Items.Include(i => i.Rarity).Select(i => new { Name = i.Name + " *" + i.Rarity.Name + "* ", i.ID }).ToListAsync();
            ViewData["Items"] = new SelectList(items, "ID", "Name");

            return View();
        }

        // POST: Admin/MonstersLoot/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MonsterLootViewModel monsterLootList)
        {
            if (ModelState.IsValid)
            {
                List<MonsterLoot> LootList = new List<MonsterLoot>();

                foreach (var item in monsterLootList.ItemIdList)
                {
                    MonsterLoot monsterLoot = new MonsterLoot()
                    {
                        ItemId = item,
                        MonsterId = monsterLootList.MonsterId
                    };
                    LootList.Add(monsterLoot);
                }
                await _context.MonstersLoot.AddRangeAsync(LootList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: Admin/MonstersLoot/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var monsterLoot = await _context.MonstersLoot.FindAsync(id);
            if (monsterLoot == null)
            {
                return NotFound();
            }

            ViewData["Monsters"] = new SelectList(_context.Monsters, "ID", "Name", monsterLoot.MonsterId);
            ViewData["Items"] = new SelectList(_context.Items, "ID", "Name", monsterLoot.ItemId);

            return View(monsterLoot);
        }

        // POST: Admin/MonstersLoot/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,MonsterId,ItemId")] MonsterLoot monsterLoot)
        {
            if (id != monsterLoot.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(monsterLoot);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MonsterLootExists(monsterLoot.ID))
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

            return View(monsterLoot);
        }

        // GET: Admin/MonstersLoot/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var monsterLoot = await _context.MonstersLoot
                .FirstOrDefaultAsync(m => m.ID == id);
            if (monsterLoot == null)
            {
                return NotFound();
            }

            return View(monsterLoot);
        }

        // POST: Admin/MonstersLoot/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var monsterLoot = await _context.MonstersLoot.FindAsync(id);
            _context.MonstersLoot.Remove(monsterLoot);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MonsterLootExists(int id)
        {
            return _context.MonstersLoot.Any(e => e.ID == id);
        }
    }
}
