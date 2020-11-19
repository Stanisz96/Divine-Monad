using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult Test()
        {
            _itemRepo.GetItemById(1).Price += 1;
            return View(_itemRepo.GetItemById(1));
        }
    }
}
