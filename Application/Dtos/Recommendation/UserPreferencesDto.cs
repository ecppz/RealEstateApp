using System.Text.Json.Serialization;

namespace Application.Dtos.Recommendation
{
    public class UserPreferencesDto
    {
        [JsonPropertyName("budget")]
        public decimal Budget { get; set; }

        [JsonPropertyName("rooms")]
        public int Rooms { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("size")]
        public int Size { get; set; }
    }
}
