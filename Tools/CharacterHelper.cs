using DivineMonad.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DivineMonad.Tools
{
    public class CharacterHelper : ICharacterHelper
    {
        public int RequiredExperience(int level)
        {
            double exp = Math.Pow(10 * level, 1.7) / 4 + 10;

            return (int)exp;
        }

        public int GetFirstEmptySlot(Character character,
            IEnumerable<CharacterItems> characterItems)
        {
            for (int n = 7; n < character.CBStats.BpSlots + 7; n++)
                if (characterItems.FirstOrDefault(i => i.BpSlotId == n) is null)
                    return n;

            return -1;
        }
    }
}
