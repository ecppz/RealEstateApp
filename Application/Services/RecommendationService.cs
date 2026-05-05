using Application.Dtos.Recommendation;
using Application.Interfaces;
using Application.Recommendation;

namespace Application.Services
{
    public sealed class RecommendationService : IRecommendationService
    {
        private readonly ScoringFunction _scoringFunction;
        private readonly ExplanationGenerator _explanationGenerator;

        public RecommendationService(ScoringFunction scoringFunction, ExplanationGenerator explanationGenerator)
        {
            _scoringFunction = scoringFunction;
            _explanationGenerator = explanationGenerator;
        }

        public RecommendResponseDto Recommend(RecommendRequestDto request)
        {
            ArgumentNullException.ThrowIfNull(request);

            if (request.Properties == null || request.Properties.Count < 2)
                throw new ArgumentException("Se requieren al menos dos propiedades para generar una recomendación.", nameof(request));

            if (request.Preferences == null || !HasAnyPreference(request.Preferences))
                throw new ArgumentException("Debes indicar al menos una preferencia (presupuesto, habitaciones, tamaño o tipo) para obtener una recomendación.", nameof(request));

            var ranked = new List<(RecommendationPropertyDto Prop, PropertyScoringBreakdown Breakdown)>();

            foreach (var p in request.Properties)
            {
                var breakdown = _scoringFunction.Score(p, request.Preferences);
                ranked.Add((p, breakdown));
            }

            ranked.Sort((a, b) =>
            {
                var cmp = b.Breakdown.TotalScore.CompareTo(a.Breakdown.TotalScore);
                return cmp != 0 ? cmp : (a.Prop.Id ?? 0).CompareTo(b.Prop.Id ?? 0);
            });

            var best = ranked[0];
            var explanation = _explanationGenerator.Generate(request.Preferences, best.Breakdown);

            return new RecommendResponseDto
            {
                RecommendedProperty = best.Prop,
                Score = best.Breakdown.TotalScore,
                Explanation = explanation,
                RankedProperties = ranked.Select(x => new RankedPropertyDto
                {
                    Property = x.Prop,
                    Score = x.Breakdown.TotalScore
                }).ToList()
            };
        }

        private static bool HasAnyPreference(UserPreferencesDto p) =>
            p.Budget > 0
            || p.Rooms > 0
            || p.Size > 0
            || !string.IsNullOrWhiteSpace(p.Type);
    }
}
