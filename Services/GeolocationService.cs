using DesktopAssignment.Models;
using System.Net.Http;
using System.Text.Json;

namespace DesktopAssignment.Services
{
    public class GeolocationService : IGeolocationService
    {
        private readonly HttpClient _httpClient;
        private string _apiKey = string.Empty;

        public GeolocationService()
        {
            _httpClient = new HttpClient();
        }

        /// <summary>  
        /// Retrieves geolocation data for the specified IP address or URL.  
        /// </summary>  
        /// <param name="ipAddressOrUrl">The IP address or URL to get geolocation data for.</param>  
        /// <returns>A task that represents the asynchronous operation. The task result contains the geolocation data.</returns>  
        public async Task<GeolocationModel> GetGeolocationDataAsync(string ipAddressOrUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(_apiKey))
                {
                    throw new InvalidOperationException("API Key is not set. Please set the API Key before making requests.");
                }

                var url = $"http://api.ipstack.com/{ipAddressOrUrl}?access_key={_apiKey}";
                var response = await _httpClient.GetStringAsync(url);
                var geolocationResponse = JsonSerializer.Deserialize<GeolocationResponseDTO>(response);

                // Validate the response  
                if (geolocationResponse == null)
                {
                    throw new Exception("Invalid response from geolocation API.");
                }

                // Validate the geolocation data  
                if (string.IsNullOrEmpty(geolocationResponse.Ip) ||
                    string.IsNullOrEmpty(geolocationResponse.Type) ||
                    string.IsNullOrEmpty(geolocationResponse.ContinentName) ||
                    string.IsNullOrEmpty(geolocationResponse.CountryName) ||
                    string.IsNullOrEmpty(geolocationResponse.RegionName) ||
                    string.IsNullOrEmpty(geolocationResponse.City) ||
                    string.IsNullOrEmpty(geolocationResponse.Zip))
                {
                    throw new Exception("The provided IP Address or URL did not return valid geolocation data. Please check the IP address or URL.");
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
            catch (JsonException)
            {
                throw new Exception("Invalid response format it may be caused by API Key, IP address, URL. Please check it and try again.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get geolocation data: {ex.Message}");
            }
        }

        public void SetApiKey(string apiKey)
        {
            _apiKey = apiKey;
        }
    }
}