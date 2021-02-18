using DivineMonad.Data;
using System.Collections.Generic;

namespace DivineMonad.Models
{
    public class CharacterRepo : ICharacterRepo
    {
        private readonly ApplicationDbContext _appDbContext;

        public CharacterRepo(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Character> AllCharacters => _appDbContext.Characters;
    }
}
