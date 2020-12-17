﻿using DivineMonad.Engine;
using DivineMonad.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DivineMonad.ViewComponents
{
    public class GameBackpackViewComponent : ViewComponent
    {
        private readonly ICharacterBaseStatsRepo _baseStats;

        public GameBackpackViewComponent(ICharacterBaseStatsRepo baseStatsRepo)
        {
            _baseStats = baseStatsRepo;
        }

        public IViewComponentResult Invoke()
        {
            return View("Empty");
        }
    }
}
