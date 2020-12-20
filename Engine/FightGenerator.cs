using DivineMonad.Engine.Raport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DivineMonad.Engine
{
    public class FightGenerator : IFightGenerator
    {
        Random rand;
        private AdvanceStats Attacker { get; set; }
        private AdvanceStats Defender { get; set; }
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

        private readonly IAdvanceStats _playerStats;
        private readonly IAdvanceStats _opponentStats;

        public FightGenerator(IAdvanceStats playerStats, IAdvanceStats opponentStats)
        {
            rand = new Random();
            Raport = new RaportGenerator();
            Raport.Rounds = new List<Round>();
            _playerStats = playerStats;
            _opponentStats = opponentStats;
        }

        public RaportGenerator GenerateFight()
        {
            IsExtraAttackDone = false;
            DoExtraAttack = false;
            IsFightOver = false;
            RoundNumber = 1;

            Action fightAction = new Action(SetMainRaportProps);
            fightAction += SetWhoStart;
            fightAction += SetStartHp;

            fightAction += UpdateBonuses;
            fightAction += UpdateDamage;
            fightAction += UpdateReceiveDamage;
            fightAction += AddRaportRound;
            fightAction += CheckIfDoubleAttack;
            fightAction += CheckIfFightOver;
            fightAction += UpdateToNextRound;

            fightAction();

            fightAction -= SetWhoStart;
            fightAction -= SetStartHp;

            while(!IsFightOver)
            {
                if (RoundNumber >= 20) IsFightOver = true;

                fightAction();
            }

            SetRaportResults();

            return Raport;
        }

        private void SetMainRaportProps()
        {
            Raport.IsPvp = _opponentStats.IsPlayer;
            Raport.Player = new Player() { ID = _playerStats.CharacterId };
            Raport.Opponent = new Opponent() { ID = _opponentStats.CharacterId };

        }

        private void SetWhoStart()
        {
            if (_playerStats.Speed >= _opponentStats.Speed)
            {
                AttackerId = _playerStats.CharacterId;
                Attacker = (AdvanceStats)_playerStats;
                DefenderId = _opponentStats.CharacterId;
                Defender = (AdvanceStats)_opponentStats;
            }
            else
            {
                AttackerId = _opponentStats.CharacterId;
                Attacker = (AdvanceStats)_opponentStats;
                DefenderId = _playerStats.CharacterId;
                Defender = (AdvanceStats)_playerStats;
            }
        }

        private void UpdateBonuses()
        { 

            if (rand.NextDouble() <= Attacker.CritPr) IsCrit = true;
            else IsCrit = false;

            if (rand.NextDouble() <= Defender.DodgePr) IsMiss = true;
            else IsMiss = false;

            if (rand.NextDouble() <= Defender.BlockPr) IsBlock = true;
            else IsBlock = false;
        }

        private void SetStartHp()
        {
            AttackerHp = Attacker.HitPoints;
            DefenderHp = Defender.HitPoints;
        }

        private void UpdateDamage()
        {
            Damage = rand.Next(Attacker.AttackMin, Attacker.AttackMax);
            if (IsCrit)
            {
                Damage = (int)(Damage * 1.5);
            }
        }

        private void UpdateReceiveDamage()
        {
            if(IsMiss)
            {
                Receive = 0;
            }
            else
            {
                Receive = (int)(Damage * (1 - Defender.DamageReduction));
                if (IsBlock) Receive = (int)(Receive * 0.5);
            }

            DefenderHp -= Receive;
        }

        private void AddRaportRound()
        {
            Raport.Rounds.Add(new Round()
            {
                Attacker = new Attacker() { Crit = IsCrit, Damage = Damage, HP = AttackerHp, ID = AttackerId, Miss = IsMiss },
                Defender = new Defender() { Block = IsBlock, Receive = Receive, HP = DefenderHp, ID = DefenderId },
                Number = RoundNumber
            });
        }

        private void CheckIfDoubleAttack()
        {
            double doubleFactor = Attacker.Speed / Defender.Speed;
            if(doubleFactor > 1 && IsExtraAttackDone == false)
            {
                double extraAttack = (RoundNumber * doubleFactor) - ((int)(RoundNumber * doubleFactor))
                    / Math.Pow(doubleFactor, 1.2) + 0.04;

                if(extraAttack <= 0.25)
                {
                    DoExtraAttack = true;
                }
            }
            else
            {
                IsExtraAttackDone = false;
            }
        }

        private void UpdateToNextRound()
        {
            RoundNumber += 1;

            if(DoExtraAttack)
            {
                IsExtraAttackDone = true;
            }

            else
            {
                AdvanceStats tempA, tempB;
                int hpA, hpB, idA, idB;
                tempA = Attacker;
                tempB = Defender;
                hpA = AttackerHp;
                hpB = DefenderHp;
                idA = AttackerId;
                idB = DefenderId;
                Attacker = tempB;
                Defender = tempA;
                AttackerHp = hpB;
                DefenderHp = hpA;
                AttackerId = idB;
                DefenderId = idA;
            }
        }

        private void CheckIfFightOver()
        {
            if(DefenderHp <= 0)
            {
                IsFightOver = true;
            }
        }
        private void SetRaportResults()
        {
            // Everything is the other way around, becouse
            // function UpdateToNextRound() change attacker to defender

            if (AttackerHp > 0) Raport.Result = "draw";
            else
            {
                if (AttackerId == _playerStats.CharacterId) Raport.Result = "lose";
                else Raport.Result = "win";
            }
        }
    }
}
