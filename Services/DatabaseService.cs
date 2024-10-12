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

        /// <summary>
        /// Ensures that the database for the context exists. If it exists, no action is taken.
        /// If it does not exist, then the database and all its schema are created.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task EnsureDatabaseExistsAsync()
        {
            await _dbContext.Database.MigrateAsync();
        }
    }
}
