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
        private readonly ICharacterItemsRepo _characterItemsRepo;
        private readonly IItemStatsRepo _itemsStatsRepo;

        public GameCharacterViewComponent(ICharacterBaseStatsRepo baseStatsRepo, ICharacterItemsRepo characterItemsRepo,
            IItemStatsRepo itemsStatsRepo)
        {
            _baseStatsRepo = baseStatsRepo;
            _characterItemsRepo = characterItemsRepo;
            _itemsStatsRepo = itemsStatsRepo;
        }

/*        public IViewComponentResult Invoke(int cId, int bsId)
        {
            var characterItems = _characterItemsRepo.GetCharactersItemsList(cId, true);
            List<int> isIds = characterItems.Select(i => i.ItemId).ToList();

            CharacterBaseStats baseStats = _baseStatsRepo.GetStatsById(bsId);
            IEnumerable<ItemStats> itemStatsList = _itemsStatsRepo.GetListStatsByIds(isIds);

            var characterAdvanceStats = new AdvanceStats();
            characterAdvanceStats.IsPlayer = true;
            characterAdvanceStats.CharacterId = cId;
            characterAdvanceStats.CalculateWithoutEq(baseStats);
            characterAdvanceStats.CalculateWithEq(itemStatsList);

            return View("Stats", characterAdvanceStats);
        }*/
    }
}
