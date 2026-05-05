using Application.Dtos.Recommendation;

namespace Application.Recommendation
{
    public sealed class ChatResponseGenerator
    {
        public string GenerateReply(
            string originalMessage,
            RecommendationPropertyDto? property,
            string recommendationExplanation,
            bool isRecommendationIntent,
            bool isExplainIntent,
            bool isPriceIntent)
        {
            if (property == null)
                return "No encontré una propiedad adecuada con los datos enviados. Intenta ajustar tus preferencias para darte una mejor sugerencia.";

            var reference = !string.IsNullOrWhiteSpace(property.Code)
                ? $"la propiedad {property.Code}"
                : "la propiedad que mejor encaja";

            if (isExplainIntent)
            {
                return $"Claro. {reference} destaca porque {recommendationExplanation}";
            }

            if (isPriceIntent)
            {
                return $"Si buscas una opción económica, te conviene {reference}: mantiene buen balance entre precio y ajuste a tus preferencias.";
            }

            if (isRecommendationIntent)
            {
                return $"Según lo que me cuentas, te recomiendo {reference} porque se adapta bien a tus preferencias de presupuesto, habitaciones y tamaño.";
            }

            return $"Entendido. Con base en tu mensaje \"{originalMessage}\", la mejor opción es {reference}. {recommendationExplanation}";
        }
    }
}
