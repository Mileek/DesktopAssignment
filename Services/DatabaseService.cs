using DesktopAssignment.Data;
using Microsoft.EntityFrameworkCore;

namespace DesktopAssignment.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly GeolocationDbContext _dbContext;

        public DatabaseService(GeolocationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task EnsureDatabaseExists()
        {
            await _dbContext.Database.MigrateAsync();
        }
    }
}
