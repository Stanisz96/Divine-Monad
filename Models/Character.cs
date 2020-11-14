using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DivineMonad.Models
{
    public class Character
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public int CStatsId { get; set; }
        public CharacterBaseStats CBStats { get; set; }
        public int GStatsId { get; set; }
        public GameStats GStats { get; set; }
    }
}
