using DivineMonad.Engine;
using DivineMonad.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DivineMonad.ViewComponents
{
    public class GameCharacterViewComponent : ViewComponent
    {
        private readonly ICharacterBaseStatsRepo _baseStats;

        public GameCharacterViewComponent(ICharacterBaseStatsRepo baseStatsRepo)
        {
            _baseStats = baseStatsRepo;
        }

        public IViewComponentResult Invoke(int id)
        {
            var advanceStats = new CharacterAdvanceStats(_baseStats, id);
            advanceStats.Calculate();

            return View("Stats",advanceStats);
        }
    }
}
