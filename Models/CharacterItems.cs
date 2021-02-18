namespace DivineMonad.Models
{
    public class CharacterItems
    {
        public int ID { get; set; }
        public int CharacterId { get; set; }
        public int ItemId { get; set; }
        public bool IsEquipped { get; set; }
        public int BpSlotId { get; set; }

        public CharacterItems()
        {
            IsEquipped = false;
        }
    }
}
