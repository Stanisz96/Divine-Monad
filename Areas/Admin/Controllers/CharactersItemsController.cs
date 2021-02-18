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
    public class CharactersItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CharactersItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var charactersItemsDbContext = await _context
                .CharactersItems.ToListAsync();

            ViewData["Characters"] = new SelectList(_context.Characters, "ID", "Name");
            ViewData["Items"] = new SelectList(_context.Items, "ID", "Name");

            return View(charactersItemsDbContext);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var characterItems = await _context.CharactersItems
                .FirstOrDefaultAsync(m => m.ID == id);

            if (characterItems == null) return NotFound();

            return View(characterItems);
        }

        public IActionResult Create()
        {
            ViewData["Characters"] = 
                new SelectList(_context.Characters, "ID", "Name");
            ViewData["Items"] = 
                new SelectList(_context.Items, "ID", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("ID,CharacterId,ItemId,IsEquipped,BpSlotId")] 
                CharacterItems characterItems)
        {
            if (ModelState.IsValid)
            {
                _context.Add(characterItems);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(characterItems);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();


            var characterItems = await _context
                .CharactersItems.FindAsync(id);

            if (characterItems == null) return NotFound();

            ViewData["Characters"] = new SelectList(
                _context.Characters, "ID", "Name", characterItems.CharacterId);
            ViewData["Items"] = new SelectList(
                _context.Items, "ID", "Name", characterItems.ItemId);

            return View(characterItems);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("ID,CharacterId,ItemId,IsEquipped,BpSlotId")]
                CharacterItems characterItems)
        {
            if (id != characterItems.ID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(characterItems);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CharacterItemsExists(characterItems.ID)) return NotFound();
                    else throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(characterItems);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var characterItems = await _context.CharactersItems
                .FirstOrDefaultAsync(m => m.ID == id);

            if (characterItems == null) return NotFound();

            return View(characterItems);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var characterItems = await _context.CharactersItems.FindAsync(id);

            _context.CharactersItems.Remove(characterItems);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool CharacterItemsExists(int id) =>
            _context.CharactersItems.Any(e => e.ID == id);
    }
}
