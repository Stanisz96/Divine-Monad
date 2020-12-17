using DivineMonad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DivineMonad.Engine
{
    public class CharacterAdvanceStats
    {
        private readonly CharacterBaseStats baseStats;
        private readonly IEnumerable<ItemStats> itemStatsList;

        public CharacterAdvanceStats(ICharacterBaseStatsRepo characterBaseStatsRepo,
            int bsId, IItemStatsRepo itemStatsRepo, List<int> isIds)
        {
            baseStats = characterBaseStatsRepo.GetStatsById(bsId);
            itemStatsList = itemStatsRepo.GetListStatsByIds(isIds);
        }

        public int Stamina { get; set; }
        public int Strength { get; set; }
        public int Agility { get; set; }
        public int Dexterity { get; set; }
        public int Luck { get; set; }
        public int HitPoints { get; set; }
        public int Attack { get; set; }
        public int AttackMin { get; set; }
        public int AttackMax { get; set; }
        public int Armor { get; set; }
        public int Block { get; set; }
        public int Dodge { get; set; }
        public int Speed { get; set; }
        public int CritChance { get; set; }
        public int Accuracy { get; set; }

        public void CalculateWithoutEq()
        {
            Stamina = baseStats.Stamina;
            Strength = baseStats.Strength;
            Dexterity = baseStats.Dexterity;
            Agility = baseStats.Agility;
            Luck = baseStats.Luck;

            HitPoints = (int)(Math.Pow(baseStats.Stamina, 1.2) * 10);
            AttackMin = 0;
            AttackMax = 0;
            Attack = (int)Math.Pow(baseStats.Strength, 1.2);
            Armor = (int)Math.Pow(baseStats.Stamina / 2, 1.2);
            Block = (int)Math.Pow(baseStats.Strength / 2, 1.2);
            Dodge = (int)(Math.Pow(baseStats.Agility, 1.2) / 5);
            Speed = (int)(Math.Pow(baseStats.Agility, 1.2) / 5) + (int)(Math.Pow(baseStats.Dexterity, 1.2) / 5);
            CritChance = (int)Math.Pow(baseStats.Luck, 1.2);
            Accuracy = (int)Math.Pow(baseStats.Dexterity, 1.2);
        }

        public void CalculateWithEq()
        {

        }
    }
}
