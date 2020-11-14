using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DivineMonad.Models
{
    public interface IItemRepo
    {
        IEnumerable<Item> AllItems { get; }
        Item GetItemById(int itemId);
    }
}
