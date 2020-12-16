using DivineMonad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DivineMonad.Models
{
    public class CharacterBaseStatsRepo : ICharacterBaseStatsRepo
    {
        private readonly ApplicationDbContext _context;

        public CharacterBaseStatsRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<CharacterBaseStats> AllChatactersBaseStats => _context.CharactersBaseStats;

        public CharacterBaseStats GetStatsById(int id)
        {
            return _context.CharactersBaseStats.FirstOrDefault(i => i.ID == id);
        }
    }
}
