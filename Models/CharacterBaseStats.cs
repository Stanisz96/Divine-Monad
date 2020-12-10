namespace DivineMonad.Models
{
    public class CharacterBaseStats
    {
        public int ID { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }
        public int Stamina { get; set; }
        public int Strength { get; set; }
        public int Agility { get; set; }
        public int Dexterity { get; set; }
        public int Luck { get; set; }

        public CharacterBaseStats()
        {
        }
    }
}