using DivineMonad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DivineMonad.Models
{
    public class ItemStatsRepo : IItemStatsRepo
    {
        public readonly ApplicationDbContext _appDbContext;

        public ItemStatsRepo(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<ItemStats> AllItemsStats => _appDbContext.ItemsStats;

        public ItemStats GetStatsById(int statsId)
        {
            return _appDbContext.ItemsStats.FirstOrDefault(i => i.ID == statsId);
        }

        public IEnumerable<ItemStats> GetListStatsByIds(List<int> ids)
        {
            return _appDbContext.ItemsStats.Where(s => ids.Contains(s.ID)).ToList();
        }
    }
}
