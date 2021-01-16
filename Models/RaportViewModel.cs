using DivineMonad.Engine.Raport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DivineMonad.Models
{
    public class RaportViewModel
    {
        public RaportGenerator Raport { get; set; }
        public Character Player { get; set; }
        public Monster Opponent { get; set; }
    }
}
