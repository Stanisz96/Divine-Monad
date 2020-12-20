using DivineMonad.Engine.Raport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DivineMonad.Engine
{
    public class FightGenerator
    {
        Random rand;
        public AdvanceStats PlayerStats { get; set; }
        public AdvanceStats OpponentStats { get; set; }
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


        public FightGenerator(AdvanceStats playerStats, AdvanceStats opponentStats)
        {
            rand = new Random();
            PlayerStats = playerStats;
            OpponentStats = opponentStats;
        }

        public void GenerateFight()
        {
            Action fightAction = new Action(SetMainRaportProps);
            fightAction += SetWhoStart;
            fightAction += SetStartHp;

            for (int i = 1; i <= 5; i++)
            {
                fightAction += UpdateBonuses;
                fightAction += UpdateDamage;
            }

            fightAction();
        }

/*        private void DealDmgToPlayer(AdvanceStats player, AdvanceStats opponent)
        {
            player.HitPoints += rand.Next(0, 10) - 4;
            HPInfo += "HP: " + opponent.HitPoints + "\n";
        }*/

        private void SetMainRaportProps()
        {
            Raport.IsPvp = OpponentStats.IsPlayer;
            Raport.Player = new Player() { ID = PlayerStats.CharacterId };
            Raport.Opponent = new Opponent() { ID = OpponentStats.CharacterId };
            Raport.Rounds = new List<Round>();
        }

        private void SetWhoStart()
        {
            if (PlayerStats.Speed >= OpponentStats.Speed)
            {
                AttackerId = PlayerStats.CharacterId;
                Attacker = PlayerStats;
                DefenderId = OpponentStats.CharacterId;
                Defender = OpponentStats;
            }
            else
            {
                AttackerId = OpponentStats.CharacterId;
                Attacker = OpponentStats;
                DefenderId = PlayerStats.CharacterId;
                Defender = PlayerStats;
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
        }
    }
}
