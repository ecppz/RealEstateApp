using Application.Dtos.Recommendation;
using System.Text.Json.Serialization;

namespace Application.Dtos.Chat
{
    public class ChatRequestDto
    {
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("preferences")]
        public UserPreferencesDto? Preferences { get; set; }

        [JsonPropertyName("properties")]
        public List<RecommendationPropertyDto>? Properties { get; set; }
    }
}
