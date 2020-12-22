using DivineMonad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DivineMonad.Engine
{
    public interface IAdvanceStats
    {
        public void CalculateWithoutEq(CharacterBaseStats baseStats);
        public void CalculateWithEq(IEnumerable<ItemStats> itemStatsList);
        public void CalculateMonster(MonsterStats monsterStats);

        public int CharacterId { get; set; }
        public bool IsPlayer { get; set; }
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

        public double CritPr { get; set; }
        public double DodgePr { get; set; }
        public double BlockPr { get; set; }
        public double ExtraDropPr { get; set; }
        public double DmgRed { get; set; }
    }
}
