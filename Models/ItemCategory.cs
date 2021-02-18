namespace DivineMonad.Models
{
    public class ItemCategory
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Armor { get; set; }
        public bool Arrow { get; set; }
        public bool Boots { get; set; }
        public bool Bow { get; set; }
        public bool Gloves { get; set; }
        public bool Helmet { get; set; }
        public bool Weapon1H { get; set; }
        public bool Weapon2H { get; set; }
        public bool Shield { get; set; }

        public ItemCategory()
        {

        }
    }
}
