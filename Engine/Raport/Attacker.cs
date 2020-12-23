using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DivineMonad.Engine.Raport
{
    public class Attacker
    {
        public string Name { get; set; }
        public int HP { get; set; }
        public bool Crit { get; set; }
        public bool Miss { get; set; }
        public int Damage { get; set; }
    }
}
