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
        private readonly ICharacterBaseStatsRepo _baseStatsRepo;
        private readonly ICharacterItemsRepo _charactersItemsRepo;
        private readonly IItemStatsRepo _itemsStatsRepo;

        public GameCharacterViewComponent(ICharacterBaseStatsRepo baseStatsRepo, ICharacterItemsRepo characterItemsRepo,
            IItemStatsRepo itemsStatsRepo)
        {
            _baseStatsRepo = baseStatsRepo;
            _charactersItemsRepo = characterItemsRepo;
            _itemsStatsRepo = itemsStatsRepo;
        }

        public IViewComponentResult Invoke(int cId, int bsId)
        {
            var characterItems = _charactersItemsRepo.GetCharactersItemsList(cId, true);
            List<int> isIds = characterItems.Select(i => i.ItemId).ToList();

            var advanceStats = new CharacterAdvanceStats(_baseStatsRepo, bsId, _itemsStatsRepo, isIds);
            advanceStats.CalculateWithoutEq();

            return View("Stats",advanceStats);
        }
    }
}
