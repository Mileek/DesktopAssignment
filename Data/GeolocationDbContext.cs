using DesktopAssignment.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace DesktopAssignment.Data
{
    public class GeolocationDbContext : DbContext
    {
        public DbSet<GeolocationModel> Geolocation { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var databasePath = Path.Combine(baseDirectory, "geolocation.db");

            optionsBuilder.UseSqlite($"Data Source={databasePath}");
        }
    }
}