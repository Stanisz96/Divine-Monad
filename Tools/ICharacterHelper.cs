using DivineMonad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DivineMonad.Tools
{
    public interface ICharacterHelper
    {
        public int RequiredExperience(int level);
        public int GetFirstEmptySlot(Character character, IEnumerable<CharacterItems> characterItems);
    }
}
