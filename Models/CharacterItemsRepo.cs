using DivineMonad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DivineMonad.Models
{
    public class CharacterItemsRepo : ICharacterItemsRepo
    {
        private readonly ApplicationDbContext _appDbContext;

        public CharacterItemsRepo(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<CharacterItems> AllCharacterItems => _appDbContext.CharactersItems;
    }
}
