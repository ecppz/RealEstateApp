using Application.Dtos.Chat;
using Application.Dtos.Recommendation;
using Application.Interfaces;
using Application.Recommendation;

namespace Application.Services
{
    public sealed class ChatService : IChatService
    {
        private readonly IRecommendationService _recommendationService;
        private readonly ChatResponseGenerator _chatResponseGenerator;

        public ChatService(
            IRecommendationService recommendationService,
            ChatResponseGenerator chatResponseGenerator)
        {
            _recommendationService = recommendationService;
            _chatResponseGenerator = chatResponseGenerator;
        }

        public ChatResponseDto ProcessMessage(ChatRequestDto request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var message = request.Message?.Trim() ?? string.Empty;
            var normalizedMessage = message.ToLowerInvariant();

            var isRecommendationIntent = ContainsAny(normalizedMessage, "recomienda", "mejor", "conviene");
            var isExplainIntent = ContainsAny(normalizedMessage, "explica", "explicame", "por que", "porqué");
            var isPriceIntent = ContainsAny(normalizedMessage, "barato", "economico", "económico", "asequible");

            var recommendation = _recommendationService.Recommend(new RecommendRequestDto
            {
                Preferences = request.Preferences,
                Properties = request.Properties
            });

            var selectedProperty = isPriceIntent
                ? SelectBestPriceFocusedProperty(recommendation, request.Properties)
                : recommendation.RecommendedProperty;

            var reply = _chatResponseGenerator.GenerateReply(
                message,
                selectedProperty,
                recommendation.Explanation,
                isRecommendationIntent,
                isExplainIntent,
                isPriceIntent);

            return new ChatResponseDto
            {
                Reply = reply,
                RecommendedProperty = selectedProperty
            };
        }

        private static bool ContainsAny(string text, params string[] keywords) =>
            keywords.Any(k => text.Contains(k, StringComparison.OrdinalIgnoreCase));

        private static RecommendationPropertyDto? SelectBestPriceFocusedProperty(
            RecommendResponseDto recommendation,
            IReadOnlyList<RecommendationPropertyDto>? sourceProperties)
        {
            if (sourceProperties == null || sourceProperties.Count == 0)
                return recommendation.RecommendedProperty;

            var minPrice = sourceProperties.Min(p => p.Price);
            var maxPrice = sourceProperties.Max(p => p.Price);

            var rankedScores = recommendation.RankedProperties?
                .Where(r => r.Property?.Id.HasValue == true)
                .ToDictionary(r => r.Property!.Id!.Value, r => r.Score)
                ?? new Dictionary<int, decimal>();

            return sourceProperties
                .Select(property =>
                {
                    var affordability = maxPrice == minPrice
                        ? 1m
                        : (maxPrice - property.Price) / (maxPrice - minPrice);

                    var recommendationScore = property.Id.HasValue && rankedScores.TryGetValue(property.Id.Value, out var score)
                        ? score / 100m
                        : 0m;

                    var finalScore = (affordability * 0.7m) + (recommendationScore * 0.3m);
                    return new { Property = property, FinalScore = finalScore };
                })
                .OrderByDescending(x => x.FinalScore)
                .ThenBy(x => x.Property.Price)
                .Select(x => x.Property)
                .FirstOrDefault();
        }
    }
}
