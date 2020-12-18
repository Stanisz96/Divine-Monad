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
    public class MonstersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MonstersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Monsters
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Monsters.Include(m => m.MonsterStats);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/Monsters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var monster = await _context.Monsters
                .Include(m => m.MonsterStats)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (monster == null)
            {
                return NotFound();
            }

            return View(monster);
        }

        // GET: Admin/Monsters/Create
        public IActionResult Create()
        {
            ViewData["MonsterStatsId"] = new SelectList(_context.MonstersStats, "ID", "ID");
            return View();
        }

        // POST: Admin/Monsters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Level,Gold,Experience,MonsterStatsId")] Monster monster)
        {
            if (ModelState.IsValid)
            {
                _context.Add(monster);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MonsterStatsId"] = new SelectList(_context.MonstersStats, "ID", "ID", monster.MonsterStatsId);
            return View(monster);
        }

        // GET: Admin/Monsters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var monster = await _context.Monsters.FindAsync(id);
            if (monster == null)
            {
                return NotFound();
            }
            ViewData["MonsterStatsId"] = new SelectList(_context.MonstersStats, "ID", "ID", monster.MonsterStatsId);
            return View(monster);
        }

        // POST: Admin/Monsters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Level,Gold,Experience,MonsterStatsId")] Monster monster)
        {
            if (id != monster.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(monster);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MonsterExists(monster.ID))
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
            ViewData["MonsterStatsId"] = new SelectList(_context.MonstersStats, "ID", "ID", monster.MonsterStatsId);
            return View(monster);
        }

        // GET: Admin/Monsters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var monster = await _context.Monsters
                .Include(m => m.MonsterStats)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (monster == null)
            {
                return NotFound();
            }

            return View(monster);
        }

        // POST: Admin/Monsters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var monster = await _context.Monsters.FindAsync(id);
            _context.Monsters.Remove(monster);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MonsterExists(int id)
        {
            return _context.Monsters.Any(e => e.ID == id);
        }
    }
}
