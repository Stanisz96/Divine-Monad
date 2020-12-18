using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DivineMonad.Data;
using DivineMonad.Models;
using System.Threading.Tasks;

namespace DivineMonad.Areas.Admin.ViewComponents
{
    public class ShowExternalPropsViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public ShowExternalPropsViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int? id, string type)
        {

            if (type.Equals("ItemStatistics"))
            {
                var item = await _context.ItemsStats.FirstOrDefaultAsync(s => s.ID == id);
                if (id != null) ViewData["id"] = item.ID;
                return View(type, item);
            }
            if (type.Equals("MonsterStatistics"))
            {
                var item = await _context.MonstersStats.FirstOrDefaultAsync(s => s.ID == id);
                if (id != null) ViewData["id"] = item.ID;
                return View(type, item);
            }

            return View("Default");
        }
    }
}
