using Application.Dtos.Recommendation;
using System.Text.Json.Serialization;

namespace Application.Dtos.Chat
{
    public class ChatResponseDto
    {
        [JsonPropertyName("reply")]
        public string Reply { get; set; } = string.Empty;

        [JsonPropertyName("recommendedProperty")]
        public RecommendationPropertyDto? RecommendedProperty { get; set; }
    }
}
