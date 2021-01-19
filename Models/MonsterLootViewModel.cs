using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
