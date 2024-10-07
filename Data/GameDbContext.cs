using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GrognardBot2.Models;

namespace GrognardBot2.Data
{
    public class GameDbContext : DbContext
    {
        private readonly string _rootDirectory;
        private readonly string _databaseFilePath;
        public DbSet<Character> Characters { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<Enemy> Enemies { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Stats> Stats { get; set; }

        // Constructor that accepts DbContextOptions
        public GameDbContext(DbContextOptions<GameDbContext> options) : base(options) 
        {
            _rootDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            _databaseFilePath = Path.Combine(_rootDirectory, "game_data.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            base.OnConfiguring(optionsBuilder); // Pass optionsBuilder directly to the base method
            optionsBuilder.UseSqlite($"Data Source={_databaseFilePath}"); // Use SQLite as your database
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Player>()
                .HasMany(p => p.Characters)
                .WithOne(c => c.Player)
                .HasForeignKey(c => c.PlayerId);

            modelBuilder.Entity<Character>()
                .HasOne(c => c.Player)
                .WithMany(p => p.Characters)
                .HasForeignKey(c => c.PlayerId);

            modelBuilder.Entity<Character>()
                .HasOne(c => c.Stats) // Assuming you add a Stats navigation property in Character
                .WithOne() // Assuming a one-to-one relationship
                .HasForeignKey<Character>(c => c.StatsId);

            // Inventory - Character: One-to-Many
            modelBuilder.Entity<InventoryItem>()
                .HasOne(i => i.Character)
                .WithMany(c => c.Inventory)
                .HasForeignKey(i => i.CharacterId);


        }

    }
}
