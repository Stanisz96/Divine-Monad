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

        public FightGenerator()
        {
            rand = new Random();
        }

        public void GenerateFight(AdvanceStats playerStats)
        {
            HPInfo = "HP: " + playerStats.HitPoints.ToString() + "\n";
            Action<AdvanceStats> fightAction = new Action<AdvanceStats>(DealDmgToPlayer);
            fightAction += DealDmgToPlayer;

            fightAction(playerStats);
        }

        private void DealDmgToPlayer(AdvanceStats playerStats)
        {
            playerStats.HitPoints += rand.Next(0, 10) - 4;
            HPInfo += "HP: " + playerStats.HitPoints + "\n";
        }
    }
}
