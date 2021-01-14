using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DivineMonad.Models
{
    public class Market
    {
        public int ID { get; set; }
        public int LevelMin { get; set; }
        public int LevelMax { get; set; }
        public int ItemId { get; set; }
    }
}
