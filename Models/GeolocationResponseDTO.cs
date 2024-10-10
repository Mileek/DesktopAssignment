using System.Text.Json.Serialization;

namespace DesktopAssignment.Models
{
    public class GeolocationResponseDTO
    {
        [JsonPropertyName("ip")]
        public required string Ip { get; set; }

        [JsonPropertyName("type")]
        public required string Type { get; set; }

        [JsonPropertyName("continent_name")]
        public required string ContinentName { get; set; }

        [JsonPropertyName("country_name")]
        public required string CountryName { get; set; }

        [JsonPropertyName("region_name")]
        public required string RegionName { get; set; }

        [JsonPropertyName("city")]
        public required string City { get; set; }

        [JsonPropertyName("zip")]
        public required string Zip { get; set; }

        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }
    }
}