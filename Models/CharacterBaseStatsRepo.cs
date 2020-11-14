using DivineMonad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DivineMonad.Models
{
    public class CharacterBaseStatsRepo : ICharacterBaseStatsRepo
    {
        private readonly ApplicationDbContext _appDbContext;

        public CharacterBaseStatsRepo(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<CharacterBaseStats> AllChatactersBaseStats => _appDbContext.CharactersBaseStats;
    }
}
