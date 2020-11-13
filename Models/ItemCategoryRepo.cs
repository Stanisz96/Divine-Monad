using DivineMonad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DivineMonad.Models
{
    public class ItemCategoryRepo : IItemCategoryRepo
    {
        private readonly ApplicationDbContext _appDbContext;

        public ItemCategoryRepo(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<ItemCategory> AllCategories => _appDbContext.Categories;
    }
}
