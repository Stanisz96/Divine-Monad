using DivineMonad.Data;
using Microsoft.EntityFrameworkCore;
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

        public async Task<CharacterBaseStats> GetStatsById(int id)
        {
            return await _context.CharactersBaseStats.FirstOrDefaultAsync(i => i.ID == id);
        }
    }
}
