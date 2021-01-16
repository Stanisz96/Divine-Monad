using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DivineMonad.Models
{
    public class RaportsViewModel
    {
        public List<string> RaportsNames { get; set; }

        public Character Character { get; set; }

        public List<Monster> MonstersList { get; set; }
    }
}
