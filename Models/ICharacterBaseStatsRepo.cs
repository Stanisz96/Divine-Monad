﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DivineMonad.Models
{
    public interface ICharacterBaseStatsRepo
    {
        IEnumerable<CharacterBaseStats> AllChatactersBaseStats { get; }

        CharacterBaseStats GetStatsById(int id);
    }
}
