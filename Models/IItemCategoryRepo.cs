using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DivineMonad.Models
{
    public interface IItemCategoryRepo
    {
        IEnumerable<ItemCategory> AllCategories { get; }
    }
}
