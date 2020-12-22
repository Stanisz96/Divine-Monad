using DivineMonad.Data;
using DivineMonad.Engine;
using DivineMonad.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DivineMonad.ViewComponents
{
    public class GameMarketViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public GameMarketViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke(int cId)
        {
            var character = _context.Characters.FirstOrDefault(i => i.ID == cId);

            return View("Empty", character);
        }
    }
}
