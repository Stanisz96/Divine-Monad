using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DivineMonad.Controllers
{
    public class CharactersController : Controller
    {

        [Authorize(Policy = "user")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
