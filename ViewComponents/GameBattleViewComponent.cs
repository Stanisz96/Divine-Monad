using DivineMonad.Engine;
using DivineMonad.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DivineMonad.ViewComponents
{
    public class GameBattleViewComponent : ViewComponent
    {
        private readonly ICharacterBaseStatsRepo _baseStatsRepo;
        private readonly ICharacterItemsRepo _characterItemsRepo;
        private readonly IItemStatsRepo _itemsStatsRepo;

        public GameBattleViewComponent(ICharacterBaseStatsRepo baseStatsRepo, ICharacterItemsRepo characterItemsRepo,
            IItemStatsRepo itemsStatsRepo)
        {
            _baseStatsRepo = baseStatsRepo;
            _itemsStatsRepo = itemsStatsRepo;
            _characterItemsRepo = characterItemsRepo;

        }

        public IViewComponentResult Invoke(int cId, int bsId)
        {
            if (cId > 0 && bsId > 0)
            {
                ViewData["cId"] = cId;
                ViewData["bsId"] = bsId;
                return View("Empty");
            }
            else return View("Fight");
        }
    }
}
