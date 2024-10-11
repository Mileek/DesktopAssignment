using DesktopAssignment.Models;
using Microsoft.EntityFrameworkCore;

namespace DesktopAssignment.Data
{
    public class GeolocationDbContext : DbContext
    {
        public DbSet<GeolocationModel> Geolocation { get; set; }

        public GeolocationDbContext(DbContextOptions<GeolocationDbContext> options)
        : base(options)
        {
        }
    }
}