using DesktopAssignment.Models;
using Microsoft.EntityFrameworkCore;

namespace DesktopAssignment.Data
{
    public class GeolocationDbContext : DbContext
    {
        public DbSet<GeolocationModel> Geolocations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //InMemory database for testing purposes
            optionsBuilder.UseInMemoryDatabase("GeolocationDb");
        }
    }
}