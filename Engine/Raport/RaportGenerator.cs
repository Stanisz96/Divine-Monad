﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DivineMonad.Engine.Raport
{
    public class RaportGenerator
    {
        public Player Player { get; set; }
        public Opponent Opponent { get; set; }
        public string Result { get; set; }
        public IList<Round> Rounds { get; set; }
    }
}