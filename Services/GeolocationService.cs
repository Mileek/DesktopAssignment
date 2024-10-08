﻿using DesktopAssignment.Models;
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
            var geolocationResponse = JsonSerializer.Deserialize<GeolocationResponseDTO>(response);

            // Map the DTO to the GeolocationModel
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
    }
}
