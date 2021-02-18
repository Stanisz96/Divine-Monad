using DivineMonad.Data;
using System.Collections.Generic;

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
