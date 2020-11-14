using DivineMonad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DivineMonad.Models
{
    public class GameStatsRepo : IGameStatsRepo
    {
        private readonly ApplicationDbContext _appDbContext;

        public GameStatsRepo(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<GameStats> AllCharactersGameStats => _appDbContext.CharactersGameStats;
    }
}
