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

        public Item GetItemById(int itemId)
        {
            return _appDbContext.Items.FirstOrDefault(i => i.ID == itemId);
        }
    }
}
