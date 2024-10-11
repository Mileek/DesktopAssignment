using DesktopAssignment.Models;

namespace DesktopAssignment.Services
{
    public interface IGeolocationService
    {
        Task<GeolocationModel> GetGeolocationDataAsync(string ipAddressOrUrl);
        void SetApiKey(string apiKey);
    }
}
