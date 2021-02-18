using DivineMonad.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DivineMonad.Models
{
    public class CharacterItemsRepo : ICharacterItemsRepo
    {
        private readonly ApplicationDbContext _appDbContext;

        public CharacterItemsRepo(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<CharacterItems> AllCharacterItems => _appDbContext.CharactersItems;

        public async Task<IEnumerable<CharacterItems>> GetCharactersItemsList(int cId, bool onlyEquipped)
        {
            if (onlyEquipped)
                return await _appDbContext.CharactersItems
                    .Where(i => (i.CharacterId == cId) && i.IsEquipped).ToListAsync();
            else
                return await _appDbContext.CharactersItems
                    .Where(i => i.CharacterId == cId).ToListAsync();
        }
    }
}
