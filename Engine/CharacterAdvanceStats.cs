using DivineMonad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DivineMonad.Engine
{
    public class CharacterAdvanceStats
    {
        private readonly CharacterBaseStats _baseStats;

        public CharacterAdvanceStats(ICharacterBaseStatsRepo characterBaseStatsRepo, int id)
        {
            _baseStats = characterBaseStatsRepo.GetStatsById(id);
        }

        public int HP { get; set; }
        public int Attack { get; set; }

        public void Calculate()
        {
            HP = _baseStats.Stamina * 10 + _baseStats.Strength * 2;
            Attack = _baseStats.Strength * 5 + _baseStats.Stamina * 1;
        }
    }
}
