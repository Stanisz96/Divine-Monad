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

    }
}
