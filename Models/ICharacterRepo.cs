using System.Collections.Generic;

namespace DivineMonad.Models
{
    public interface ICharacterRepo
    {
        IEnumerable<Character> AllCharacters { get; }

    }
}
