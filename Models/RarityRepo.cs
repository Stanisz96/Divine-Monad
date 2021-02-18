using DivineMonad.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DivineMonad.Models
{
    public class RarityRepo : IRarityRepo
    {
        private readonly ApplicationDbContext _appDbContext;

        public RarityRepo(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IEnumerable<Rarity>> GetRaritiesList()
        {
            return await _appDbContext.Rarity.ToListAsync();
        }
        public Rarity GetRarity(string name)
        {
            return _appDbContext.Rarity.FirstOrDefault(r => r.Name == name);
        }
    }
}
