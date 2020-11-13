using DivineMonad.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DivineMonad.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ApplicationDbContext>>()))
            {
                if (context.Items.Any())
                {
                    return;
                }

                context.Categories.AddRange(
                    new ItemCategory { Name = "Armor", Description = "Protective covering that is used to prevent damage from being inflicted to an object" },
                    new ItemCategory { Name = "Boots", Description = "A boot is a type of footwear" },
                    new ItemCategory { Name = "Helmet", Description = "A helmet is a form of protective gear worn to protect the head." }
                );

                context.Items.AddRange(
                    new Item
                    {
                        CategoryId = 2,
                        Name = "Peasant armor",
                        Description = "Ragged peasant armor that was abandoned many years ago.",
                        ImageUrl = "~/images/grey/Armor/PeasantClothing.png",
                        Price = 10,
                        Quantity = 1,
                        Statistics = new Dictionary<string, int>
                        {
                            { "HP", 10 },
                            { "Armor", 5 },
                            { "Level", 1 }
                        },
                        Category = context.Categories.FirstOrDefault(c => c.ID == 1),
                    },
                    new Item
                    {
                        CategoryId = 3,
                        Name = "Peasant boots",
                        Description = "The smelly worn-out boots of a peasant.",
                        ImageUrl = "~/images/grey/Boots/PeasantClothing.png",
                        Price = 7,
                        Quantity = 5,
                        Statistics = new Dictionary<string, int>
                        {
                            { "HP", 5 },
                            { "Armor", 2 },
                            { "Level", 1 },
                            { "Speed", 1 }
                        },
                        Category = context.Categories.FirstOrDefault(c => c.ID == 2),
                    }
                );

                context.SaveChanges();
            }
        }
    }
}
