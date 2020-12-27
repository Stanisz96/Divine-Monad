namespace DivineMonad.Models
{
    public class CharacterBaseStats
    {
        public int ID { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }
        public int Gold { get; set; }
        public int BpSlots { get; set; }
        public int Stamina { get; set; }
        public int Strength { get; set; }
        public int Agility { get; set; }
        public int Dexterity { get; set; }
        public int Luck { get; set; }

        public CharacterBaseStats()
        {
        }

        public CharacterBaseStats(string option)
        {
            if(option.Equals("new"))
            {
                Level = 1;
                Experience = 0;
                Gold = 0;
                BpSlots = 18;
                Stamina = 10;
                Strength = 10;
                Agility = 10;
                Dexterity = 10;
                Luck = 10;
            }

        }
    }
}