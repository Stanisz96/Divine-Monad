using System.Collections.Generic;
using System.Threading.Tasks;

namespace DivineMonad.Models
{
    public interface IItemStatsRepo
    {
        IEnumerable<ItemStats> AllItemsStats { get; }
        Task<IEnumerable<ItemStats>> GetListStatsByIds(List<int> ids);
        Task<ItemStats> GetStatsById(int statsId);
    }
}
