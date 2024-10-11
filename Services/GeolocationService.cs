using DesktopAssignment.Models;
using System.Net.Http;
using System.Text.Json;

namespace DesktopAssignment.Services
{
    public class GeolocationService : IGeolocationService
    {
        private readonly HttpClient _httpClient;
        private string _apiKey;

        public GeolocationService()
        {
            _httpClient = new HttpClient();
        }
        public void SetApiKey(string apiKey)
        {
            _apiKey = apiKey;
        }

        public async Task<GeolocationModel> GetGeolocationDataAsync(string ipAddressOrUrl)
        {
            try
            {
                var url = $"http://api.ipstack.com/{ipAddressOrUrl}?access_key={_apiKey}";
                var response = await _httpClient.GetStringAsync(url);
                var geolocationResponse = JsonSerializer.Deserialize<GeolocationResponseDTO>(response);

                if (geolocationResponse == null)
                {
                    throw new Exception("Invalid response from geolocation API.");
                }

                //Map the DTO to the GeolocationModel
                var geolocationModel = new GeolocationModel
                {
                    Ip = geolocationResponse.Ip,
                    Type = geolocationResponse.Type,
                    ContinentName = geolocationResponse.ContinentName,
                    CountryName = geolocationResponse.CountryName,
                    RegionName = geolocationResponse.RegionName,
                    City = geolocationResponse.City,
                    Zip = geolocationResponse.Zip,
                    Latitude = geolocationResponse.Latitude,
                    Longitude = geolocationResponse.Longitude
                };

                return geolocationModel;
            }
            catch (JsonException jsonEx)
            {
                //Apologize for no NLog implementation
                //JSON deserialization exceptions
                throw new Exception("Invalid API Key or response format. Please check your API Key and try again.");
            }
            catch (Exception ex)
            {
                
                throw new Exception($"Failed to get geolocation data: {ex.Message}");
            }
        }
    }
}
