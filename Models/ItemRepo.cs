using DivineMonad.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DivineMonad.Models
{
    public class ItemRepo : IItemRepo
    {
        private readonly ApplicationDbContext _appDbContext;

        public ItemRepo(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Item> AllItems
        {
            get
            {
                return _appDbContext.Items.Include(c => c.Category);
            }
        }

        public async Task<Item> GetItemById(int itemId)
        {
            return await _appDbContext.Items.Include(s => s.Category)
                .Include(s => s.Statistics).Include(s => s.Rarity).FirstOrDefaultAsync(i => i.ID == itemId);
        }

        public async Task<IEnumerable<Item>> GetItemsList(List<int> ids)
        {
            return await _appDbContext.Items.Where(s => ids.Contains(s.ID)).Include(s => s.Category)
                .Include(s => s.Statistics).Include(s => s.Rarity).ToListAsync();
        }
    }
}
