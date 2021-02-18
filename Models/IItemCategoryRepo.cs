using System.Collections.Generic;

namespace DivineMonad.Models
{
    public interface IItemCategoryRepo
    {
        IEnumerable<ItemCategory> AllCategories { get; }
    }
}
