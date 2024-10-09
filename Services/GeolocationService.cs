using DesktopAssignment.Models;
using System.Net.Http;
using System.Text.Json;

namespace DesktopAssignment.Services
{
    public class GeolocationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public GeolocationService(string apiKey)
        {
            _httpClient = new HttpClient();
            _apiKey = apiKey;
        }

        public async Task<GeolocationModel> GetGeolocationDataAsync(string ipAddressOrUrl)
        {
            var url = $"http://api.ipstack.com/{ipAddressOrUrl}?access_key={_apiKey}";
            var response = await _httpClient.GetStringAsync(url);
            var geolocation = JsonSerializer.Deserialize<GeolocationModel>(response);
            return geolocation;
        }
    }
}
