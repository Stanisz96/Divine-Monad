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
    public class CharactersGameStatsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CharactersGameStatsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Characters"] = new SelectList(
                _context.Characters, "GStatsId", "Name");
            ViewData["CharactersId"] = new SelectList(
                _context.Characters, "GStatsId", "ID");

            return View(await _context.CharactersGameStats.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var gameStats = await _context.CharactersGameStats
                .FirstOrDefaultAsync(m => m.ID == id);

            if (gameStats == null) return NotFound();

            ViewData["CharacterName"] = _context.Characters
                .FirstOrDefault(c => c.GStatsId == id).Name;
            ViewData["CharacterId"] = _context.Characters
                .FirstOrDefault(c => c.GStatsId == id).ID;

            return View(gameStats);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("ID,MonsterKills,CollectedGold,DeathsNumber," +
            "LostFights,WinFights,DrawFights,LootedNormal," +
            "LootedUnique,LootedHeroic,LootedLegendary")] 
                GameStats gameStats)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gameStats);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(gameStats);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var gameStats = await _context
                .CharactersGameStats.FindAsync(id);

            if (gameStats == null) return NotFound();

            return View(gameStats);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("ID,MonsterKills,CollectedGold,DeathsNumber," +
            "LostFights,WinFights,DrawFights,LootedNormal," +
            "LootedUnique,LootedHeroic,LootedLegendary")] 
                GameStats gameStats)
        {
            if (id != gameStats.ID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gameStats);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameStatsExists(gameStats.ID)) return NotFound();
                    else throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(gameStats);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var gameStats = await _context.CharactersGameStats
                .FirstOrDefaultAsync(m => m.ID == id);

            if (gameStats == null) return NotFound();

            return View(gameStats);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gameStats = await _context
                .CharactersGameStats.FindAsync(id);

            _context.CharactersGameStats.Remove(gameStats);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool GameStatsExists(int id) =>
            _context.CharactersGameStats.Any(e => e.ID == id);
    }
}
