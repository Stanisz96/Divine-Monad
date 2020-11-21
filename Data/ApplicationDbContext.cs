using System;
using System.Collections.Generic;
using System.Text;
using DivineMonad.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DivineMonad.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<ItemStats> ItemsStats { get; set; }
        public DbSet<CharacterItems> CharactersItems { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<CharacterBaseStats> CharactersBaseStats { get; set; }
        public DbSet<GameStats> CharactersGameStats { get; set; }
        public DbSet<DivineMonad.Models.Rarity> Rarity { get; set; }
    }
}
