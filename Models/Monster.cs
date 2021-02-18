using System.ComponentModel.DataAnnotations;

namespace DivineMonad.Models
{
    public class Monster
    {
        public int ID { get; set; }

        [RegularExpression(@"[a-zA-Z0-9\s]*$"), Required, StringLength(20)]
        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public int Level { get; set; }

        public int Gold { get; set; }

        public int Experience { get; set; }

        public int MonsterStatsId { get; set; }
        public MonsterStats MonsterStats { get; set; }
    }
}
