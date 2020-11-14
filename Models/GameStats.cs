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
    }
}