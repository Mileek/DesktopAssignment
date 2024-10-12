namespace DesktopAssignment.Services
{
    public interface IDatabaseService
    {
        Task EnsureDatabaseExistsAsync();
    }
}
