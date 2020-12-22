﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DivineMonad.Models
{
    public interface ICharacterItemsRepo
    {
        IEnumerable<CharacterItems> AllCharacterItems { get; }
        Task<IEnumerable<CharacterItems>> GetCharactersItemsList(int cId, bool onlyEquipped);
    }
}
