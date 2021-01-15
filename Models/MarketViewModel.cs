using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DivineMonad.Models
{
    public class MarketViewModel
    {
        public IEnumerable<Market> MarketItems { get; set; }

        public IEnumerable<CharacterItems> CharacterItemsList { get; set; }

        public Character Character { get; set; }

        public IEnumerable<Item> ItemsList { get; set; }
    }
}
