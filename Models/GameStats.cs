namespace DivineMonad.Models
{
    public class GameStats
    {
        public int ID { get; set; }
        public int MonsterKills { get; set; }
        public int CollectedGold { get; set; }
        public int DeathsNumber { get; set; }
        public int LostFights { get; set; }
        public int WinFights { get; set; }
        public int DrawFights { get; set; }
        public int LootedNormal { get; set; }
        public int LootedUnique { get; set; }
        public int LootedHeroic { get; set; }
        public int LootedLegendary { get; set; }

        public GameStats()
        {
        }

        public GameStats(string option)
        {
            if (option.Equals("new"))
            {
                MonsterKills = 0;
                CollectedGold = 0;
                DeathsNumber = 0;
                LostFights = 0;
                WinFights = 0;
                DrawFights = 0;
                LootedHeroic = 0;
                LootedLegendary = 0;
                LootedNormal = 0;
                LootedUnique = 0;
            }
        }
    }
}