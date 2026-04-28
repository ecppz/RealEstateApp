using System.Text.Json.Serialization;

namespace Application.Dtos.Recommendation
{
    public class RankedPropertyDto
    {
        [JsonPropertyName("property")]
        public RecommendationPropertyDto Property { get; set; } = null!;

        [JsonPropertyName("score")]
        public decimal Score { get; set; }
    }
}
