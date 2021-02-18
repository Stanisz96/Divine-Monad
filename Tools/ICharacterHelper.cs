using DivineMonad.Models;
using System.Collections.Generic;

namespace DivineMonad.Tools
{
    public interface ICharacterHelper
    {
        public int RequiredExperience(int level);
        public int GetFirstEmptySlot(Character character, IEnumerable<CharacterItems> characterItems);
    }
}
