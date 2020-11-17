using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DivineMonad.Models
{
    public class Character
    {
        public int ID { get; set; }

        [RegularExpression(@"[a-zA-Z0-9\s]*$"), Required, StringLength(20)]
        public string Name { get; set; }

        public string UserId { get; set; }

        public int CBStatsId { get; set; }
        public CharacterBaseStats CBStats { get; set; }

        public int GStatsId { get; set; }
        public GameStats GStats { get; set; }

        public Character()
        {
            CBStats = new CharacterBaseStats();
            GStats = new GameStats();
        }
    }
}
