namespace DivineMonad.Models
{
    public class Item
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int Price { get; set; }
        public int Level { get; set; }
        public int CategoryId { get; set; }
        public ItemCategory Category { get; set; }
        public int StatisticsId { get; set; }
        public ItemStats Statistics { get; set; }
        public int RarityId { get; set; }
        public Rarity Rarity { get; set; }

        public Item()
        {
        }
    }
}
