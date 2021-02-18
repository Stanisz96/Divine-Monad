using DivineMonad.Data;
using DivineMonad.Engine.Raport;
using DivineMonad.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DivineMonad.Tools
{
    public interface IDbContextHelper
    {
        public Task<Character> GetCharacter(int cId, ClaimsPrincipal user, ApplicationDbContext context);
        public Task<Monster> GetMonster(int mId, ApplicationDbContext context);
        public bool CanPutItOn(int from, int to, Backpack backpack);
        public bool CanMoveIt(int from, int to, Backpack backpack);
        public bool CanChangeIt(int from, int to, Backpack backpack);
        public Task<List<CharacterItems>> UpdateBpSlotsId(int from, int to, Backpack backpack,
            ApplicationDbContext context, string option);
        public (Character, CharacterItems) AssignRewards(RaportGenerator raport, Character character,
            ApplicationDbContext context, ICharacterHelper characterHelper,
            IEnumerable<CharacterItems> characterItems);
        enum CategoryName { };

    }
}
