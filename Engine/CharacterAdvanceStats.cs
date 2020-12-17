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

            Recalculate();
        }

        public void CalculateWithEq()
        {
            foreach (var item in itemStatsList)
            {
                Stamina += item.Stamina;
                Strength += item.Strength;
                Dexterity += item.Dexterity;
                Agility += item.Agility;
                Luck += item.Luck;
            }

            Recalculate();

            foreach (var item in itemStatsList)
            {
                HitPoints += item.HitPoints;
                AttackMin += item.AttackMin;
                AttackMax += item.AttackMax;
                Armor += item.Armor;
                Block += item.Block;
                Dodge += item.Dodge;
                Speed += item.Speed;
                CritChance += item.CritChance;
                Accuracy += item.Accuracy;
            }

            AttackMin = (int)((0.8 + (Math.Sqrt(Accuracy) / 100)) * Attack);
        }


        private void Recalculate()
        {
            HitPoints = (int)(Math.Pow(Stamina, 1.2) * 10);
            Armor = (int)Math.Pow(Stamina / 2, 1.2);
            Block = (int)Math.Pow(Strength / 2, 1.2);
            Dodge = (int)(Math.Pow(Agility, 1.2) / 5);
            Speed = (int)(Math.Pow(Agility, 1.2) / 5) + (int)(Math.Pow(Dexterity, 1.2) / 5);
            CritChance = (int)Math.Pow(Luck, 1.2);
            Accuracy = (int)Math.Pow(Dexterity, 1.2);
            Attack = (int)Math.Pow(Strength, 1.2);
            AttackMin = (int)((0.8 + Math.Sqrt(Accuracy) / 100) * Attack);
            AttackMax = (int)(1.1 * Attack);
        }
    }
}
