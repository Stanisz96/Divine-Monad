using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DivineMonad.Models
{
    public class CharacterItems
    {
        public int ID { get; set; }
        public int CharacterId { get; set; }
        public int ItemId { get; set; }
        public bool IsEquipped { get; set; }

        public CharacterItems()
        {
            IsEquipped = false;
        }
    }
}
