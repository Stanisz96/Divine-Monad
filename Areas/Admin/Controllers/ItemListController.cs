using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DivineMonad.Areas.Admin.Controllers
{
    public class ItemListController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
