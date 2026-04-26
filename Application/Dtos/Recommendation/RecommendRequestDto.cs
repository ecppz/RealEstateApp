using System.Text.Json.Serialization;

namespace Application.Dtos.Recommendation
{
    public class RecommendRequestDto
    {
        [JsonPropertyName("preferences")]
        public UserPreferencesDto? Preferences { get; set; }

        [JsonPropertyName("properties")]
        public List<RecommendationPropertyDto>? Properties { get; set; }
    }
}
