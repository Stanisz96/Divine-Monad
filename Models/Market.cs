namespace DivineMonad.Models
{
    public class Market
    {
        public int ID { get; set; }
        public int LevelMin { get; set; }
        public int LevelMax { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }
    }
}
