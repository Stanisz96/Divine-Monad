using DivineMonad.Data;
using Microsoft.EntityFrameworkCore;
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

        public async Task<ItemStats> GetStatsById(int statsId)
        {
            return await _appDbContext.ItemsStats.FirstOrDefaultAsync(i => i.ID == statsId);
        }

        public async Task<IEnumerable<ItemStats>> GetListStatsByIds(List<int> ids)
        {
            return await _appDbContext.ItemsStats.Where(s => ids.Contains(s.ID)).ToListAsync();
        }
    }
}
