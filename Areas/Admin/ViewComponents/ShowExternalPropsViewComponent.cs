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

        public async Task<IViewComponentResult> InvokeAsync(int? id)
        {
            var itemStats = await _context.ItemsStats.FirstOrDefaultAsync(s => s.ID == id);
            if(id != null)
                ViewData["id"] = itemStats.ID;

            return View(itemStats);
        }
    }
}
