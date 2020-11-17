using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DivineMonad.Models
{
    public class ItemStats
    {
        public int ID { get; set; }
        public int Stamina { get; set; }
        public int Strength { get; set; }
        public int Agility { get; set; }
        public int Dexterity { get; set; }
        public int Luck { get; set; }
        public int HitPoints { get; set; }
        public int AttackMin { get; set; }
        public int AttackMax { get; set; }
        public int Armor { get; set; }
        public int Block { get; set; }
        public int Dodge { get; set; }
        public int Speed { get; set; }
        public int CritChance { get; set; }
        public int Accuracy { get; set; }

        public ItemStats()
        {
            Stamina = 0;
            Strength = 0;
            Agility = 0;
            Dexterity = 0;
            Luck = 0;
            HitPoints = 0;
            AttackMin = 0;
            AttackMax = 0;
            Armor = 0;
            Block = 0;
            Dodge = 0;
            Speed = 0;
            CritChance = 0;
            Accuracy = 0;
        }
    }
}
