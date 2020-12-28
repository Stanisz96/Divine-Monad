using DivineMonad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DivineMonad.Models
{
    public class Backpack
    {
        public Character Character { get; set; }

        public IEnumerable<CharacterItems> CharacterItemsList { get; set; }

        public IEnumerable<Item> ItemsList { get; set; }
    }
}
