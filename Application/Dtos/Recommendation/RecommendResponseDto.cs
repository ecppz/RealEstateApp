using System.Text.Json.Serialization;

namespace Application.Dtos.Recommendation
{
    public class RecommendResponseDto
    {
        [JsonPropertyName("recommendedProperty")]
        public RecommendationPropertyDto? RecommendedProperty { get; set; }

        [JsonPropertyName("score")]
        public decimal Score { get; set; }

        [JsonPropertyName("explanation")]
        public string Explanation { get; set; } = string.Empty;

        [JsonPropertyName("rankedProperties")]
        public IReadOnlyList<RankedPropertyDto>? RankedProperties { get; set; }
    }
}
