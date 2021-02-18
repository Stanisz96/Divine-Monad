using DivineMonad.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DivineMonad.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly IItemRepo _itemRepo;

        public HomeController(IItemRepo itemRepo)
        {
            _itemRepo = itemRepo;
        }

        public IActionResult Index() => View();

        public IActionResult Test() => View();
    }
}
