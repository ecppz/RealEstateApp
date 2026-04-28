using System.Text.Json.Serialization;

namespace Application.Dtos.Recommendation
{
    public class RecommendationPropertyDto
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("code")]
        public string? Code { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("rooms")]
        public int Rooms { get; set; }

        [JsonPropertyName("size")]
        public int Size { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }
    }
}
