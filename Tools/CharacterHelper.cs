using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DivineMonad.Tools
{
    public class CharacterHelper : ICharacterHelper
    {
        public int RequiredExperience(int level)
        {
            double exp = Math.Pow(10 * level, 1.7) / 4 + 10;
            return (int)exp;
        }
    }
}
