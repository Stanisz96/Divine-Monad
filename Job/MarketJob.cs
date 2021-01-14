using DivineMonad.Data;
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
        private readonly IServiceScopeFactory _scopeFactory;

        public MarketJob(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var characterStats = dbContext.CharactersBaseStats.Where(s => s.ID == 1).FirstOrDefault();
                characterStats.Gold += 1;
                dbContext.Update(characterStats);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
