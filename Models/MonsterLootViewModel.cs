using System.Collections.Generic;

namespace DivineMonad.Models
{
    public class MonsterLootViewModel
    {
        public int MonsterId { get; set; }
        public IList<int> ItemIdList { get; set; }

        public MonsterLootViewModel()
        {
            ItemIdList = new List<int>();
        }
    }
}
