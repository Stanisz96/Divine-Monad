using DivineMonad.Data;
using System.Collections.Generic;

namespace DivineMonad.Models
{
    public class ItemCategoryRepo : IItemCategoryRepo
    {
        private readonly ApplicationDbContext _appDbContext;

        public ItemCategoryRepo(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<ItemCategory> AllCategories => _appDbContext.ItemCategories;
    }
}
