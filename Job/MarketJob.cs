using DivineMonad.Data;
using DivineMonad.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DivineMonad.Job
{
    [DisallowConcurrentExecution]
    public class MarketJob : IJob
    {
        private readonly ApplicationDbContext _context;

        public MarketJob(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            Random rand = new Random();
            int index = 0, count = 0;
            double gachiaDraw = 0;

            _context.Markets.RemoveRange(_context.Markets);
            await _context.SaveChangesAsync();
            var markets = await _context.Markets.ToListAsync();
            var items = await _context.Items.ToListAsync();
            var rarities = await _context.Rarity.ToListAsync();

            for (int i = 0; i < 15; i++)
            {
                gachiaDraw = rand.NextDouble() * 1000;

                foreach (var rarity in rarities.OrderBy(r => r.Chance))
                {
                    if (gachiaDraw <= rarity.Chance)
                    {
                        count = items.Where(i => i.Rarity.Name == rarity.Name).Count();
                        if (count > 0)
                        {
                            Market marketItem = new Market() { LevelMin = 1, LevelMax = 99 };

                            index = rand.Next(0, count);
                            marketItem.ItemId = items.Where(i => i.Rarity.Name == rarity.Name).ElementAt(index).ID;
                            
                            if (markets.Count < 9)
                                markets.Add(marketItem);
                        }
                        break;
                    }
                }
            }
            await _context.AddRangeAsync(markets);
            await _context.SaveChangesAsync();
        }
    }
}
