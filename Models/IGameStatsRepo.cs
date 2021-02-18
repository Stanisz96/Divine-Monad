using System.Collections.Generic;

namespace DivineMonad.Models
{
    public interface IGameStatsRepo
    {
        IEnumerable<GameStats> AllCharactersGameStats { get; }
    }
}
