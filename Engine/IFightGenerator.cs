﻿using DivineMonad.Engine.Raport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DivineMonad.Engine
{
    public interface IFightGenerator
    {
        public RaportGenerator GenerateFight();

        public RaportGenerator Raport { get; set; }
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
