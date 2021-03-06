﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace DivineMonad.Models
{
    public interface IRarityRepo
    {
        Task<IEnumerable<Rarity>> GetRaritiesList();
        Rarity GetRarity(string name);
    }
}
