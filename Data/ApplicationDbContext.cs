using System;
using System.Collections.Generic;
using System.Text;
using DivineMonad.Engine.Raport;
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
        public DbSet<Rarity> Rarity { get; set; }
        public DbSet<Monster> Monsters { get; set; }
        public DbSet<MonsterLoot> MonstersLoot { get; set; }
        public DbSet<MonsterStats> MonstersStats { get; set; }
        public DbSet<Market> Markets { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Character>()
                   .HasIndex(u => u.Name)
                   .IsUnique();
        }
    }
}
