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
        public string HPInfo { get; set; }
        public AdvanceStats PlayerStats { get; set; }
        public AdvanceStats OpponentStats { get; set; }
        public RaportGenerator Raport { get; set; }

        public FightGenerator(AdvanceStats playerStats, AdvanceStats opponentStats)
        {
            rand = new Random();
            PlayerStats = playerStats;
            OpponentStats = opponentStats;
        }

        public void GenerateFight()
        {
            HPInfo = "HP: " + PlayerStats.HitPoints.ToString() + "\n";
            Action<AdvanceStats, AdvanceStats> fightAction = new Action<AdvanceStats, AdvanceStats>(DealDmgToPlayer);
            fightAction += DealDmgToPlayer;

            fightAction(PlayerStats, OpponentStats);
        }

        private void DealDmgToPlayer(AdvanceStats player, AdvanceStats opponent)
        {
            player.HitPoints += rand.Next(0, 10) - 4;
            HPInfo += "HP: " + opponent.HitPoints + "\n";
        }

        private void SetMainRaportProps(AdvanceStats player, AdvanceStats opponent)
        {
            Raport.IsPvp = opponent.IsPlayer;
            Raport.Player = new Player() { ID = player.CharacterId };
        }
    }
}
