﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DivineMonad.Models
{
    public interface IItemRepository
    {
        IEnumerable<Item> AllItems { get; }
        Item GetItemById(int itemId);
    }
}
