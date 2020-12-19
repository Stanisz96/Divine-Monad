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

        public IViewComponentResult Invoke(int? cId, int? bsId)
        {
            if (cId.HasValue && bsId.HasValue)
            {
                var characterItems = _characterItemsRepo.GetCharactersItemsList((int)cId, true);
                List<int> isIds = characterItems.Select(i => i.ItemId).ToList();

                CharacterBaseStats baseStats = _baseStatsRepo.GetStatsById((int)bsId);
                IEnumerable<ItemStats> itemStatsList = _itemsStatsRepo.GetListStatsByIds(isIds);

                var characterAdvanceStats = new AdvanceStats();
                characterAdvanceStats.CalculateWithoutEq(baseStats);
                characterAdvanceStats.CalculateWithEq(itemStatsList);
                return View("Empty", characterAdvanceStats);
            }
            else
            {
                return View("Fight");
            }
        }
    }
}
