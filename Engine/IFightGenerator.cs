using DivineMonad.Engine.Raport;
using DivineMonad.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DivineMonad.Engine
{
    public interface IFightGenerator
    {
        public Task<RaportGenerator> GenerateFight();

        public RaportGenerator Raport { get; set; }
        public Monster Monster { get; set; }
        public IEnumerable<Item> Items { get; set; }
        public IEnumerable<Rarity> Rarities { get; set; }
        public Item ItemLooted { get; set; }
        public int AttackerId { get; set; }
        public int DefenderId { get; set; }
        public int AttackerHp { get; set; }
        public int DefenderHp { get; set; }
        public bool IsCrit { get; set; }
        public bool IsBlock { get; set; }
        public bool IsMiss { get; set; }
        public int Damage { get; set; }
        public int Receive { get; set; }
        public int RoundNumber { get; set; }
        public bool IsExtraAttackDone { get; set; }
        public bool DoExtraAttack { get; set; }
        public bool IsFightOver { get; set; }
    }
}
