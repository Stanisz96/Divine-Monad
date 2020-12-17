using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DivineMonad.Models
{
    public interface IItemStatsRepo
    {
        IEnumerable<ItemStats> AllItemsStats { get; }
        IEnumerable<ItemStats> GetListStatsByIds(List<int> ids);
        ItemStats GetStatsById(int statsId);
    }
}
