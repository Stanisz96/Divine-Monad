using System.Collections.Generic;

namespace DivineMonad.Models
{
    public class Backpack
    {
        public Character Character { get; set; }

        public IEnumerable<CharacterItems> CharacterItemsList { get; set; }

        public IEnumerable<Item> ItemsList { get; set; }
    }
}
