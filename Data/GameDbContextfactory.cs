using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrognardBot2.Data
{
    public class GameDbContextFactory : IDesignTimeDbContextFactory<GameDbContext>
    {
        
        public GameDbContext CreateDbContext(string[] args = null)
        {
            var _rootDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            var databaseFilePath = Path.Combine(_rootDirectory, "game_data.db");
            var optionsBuilder = new DbContextOptionsBuilder<GameDbContext>();
            optionsBuilder.UseSqlite($"Data Source={databaseFilePath}");

            return new GameDbContext(optionsBuilder.Options);
        }
    }
}
