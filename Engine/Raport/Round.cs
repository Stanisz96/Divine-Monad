using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DivineMonad.Engine.Raport
{
    public class Round
    {
        public int Number { get; set; }
        public Attacker Attacker { get; set; }
        public Defender Defender { get; set; }
    }
}
