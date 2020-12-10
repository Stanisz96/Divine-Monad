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
    }
}