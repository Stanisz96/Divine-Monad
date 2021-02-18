using System.Collections.Generic;
using System.Threading.Tasks;

namespace DivineMonad.Models
{
    public interface IItemRepo
    {
        IEnumerable<Item> AllItems { get; }
        Task<Item> GetItemById(int itemId);
        Task<IEnumerable<Item>> GetItemsList(List<int> ids);
    }
}
