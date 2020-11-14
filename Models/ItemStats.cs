using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DivineMonad.Models
{
    public class ItemStats
    {
        public int ID { get; set; }
        public int? HP { get; set; }
        public int? AttackMin { get; set; }
        public int? AttackMax { get; set; }
        public int? Armor { get; set; }
        public int? Critic { get; set; }
        public int? Strength { get; set; }
        public int? Stamina { get; set; }
        public int? Agility { get; set; }
        public int? AllAtributes { get; set; }
    }
}
