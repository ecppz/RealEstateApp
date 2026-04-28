using Application.Dtos.Recommendation;

namespace Application.Recommendation
{
    public sealed class ExplanationGenerator
    {
        public string Generate(
            UserPreferencesDto preferences,
            PropertyScoringBreakdown breakdown)
        {
            var factors = new List<string>();

            if (breakdown.PriceActive)
                factors.Add(DescribePrice(preferences.Budget, breakdown.PriceScore));

            if (breakdown.RoomsActive)
                factors.Add(DescribeRooms(preferences.Rooms, breakdown.RoomsScore));

            if (breakdown.SizeActive)
                factors.Add(DescribeSize(preferences.Size, breakdown.SizeScore));

            if (breakdown.TypeActive)
                factors.Add(DescribeType(breakdown.TypeScore));

            if (factors.Count == 0)
                return "No hay suficientes preferencias para generar una explicación detallada.";

            foreach (var filler in GetGenericFillers())
            {
                if (factors.Count >= 3) break;
                factors.Add(filler);
            }

            var intro = "Esta propiedad es la mejor opción porque ";
            var body = JoinSpanishList(factors.Take(3).ToList());
            return $"{intro}{body}.";
        }

        private static IEnumerable<string> GetGenericFillers()
        {
            yield return "obtiene la mejor puntuación global al compararla con el resto de opciones";
            yield return "concentra con mayor claridad lo que buscas dentro del listado enviado";
            yield return "representa el equilibrio más favorable según la lógica de puntuación aplicada";
        }

        private static string DescribePrice(decimal budget, decimal score)
        {
            if (score >= 0.85m)
                return "se ajusta bien a tu presupuesto";
            if (score >= 0.55m)
                return "se acerca razonablemente a tu presupuesto";
            return "presenta desafíos respecto a tu presupuesto, aunque sigue siendo la opción más equilibrada del conjunto";
        }

        private static string DescribeRooms(int preferred, decimal score)
        {
            if (score >= 0.85m)
                return $"tiene la cantidad de habitaciones que buscas (alrededor de {preferred})";
            if (score >= 0.55m)
                return "tiene un número de habitaciones cercano a lo que indicaste";
            return "aunque no coincide del todo con el número de habitaciones ideal, es la que mejor encaja entre las opciones";
        }

        private static string DescribeSize(int preferredM2, decimal score)
        {
            if (score >= 0.85m)
                return $"cuenta con un tamaño adecuado (cerca de {preferredM2} m²) para tus necesidades";
            if (score >= 0.55m)
                return "ofrece una superficie razonablemente alineada con lo que necesitas";
            return "en tamaño es la alternativa más cercana a tus expectativas dentro de la lista";
        }

        private static string DescribeType(decimal score)
        {
            return score >= 0.85m
                ? "coincide con el tipo de propiedad que prefieres"
                : "es la opción más compatible con el tipo de propiedad que buscas entre las enviadas";
        }

        private static string JoinSpanishList(IReadOnlyList<string> items)
        {
            if (items.Count == 1) return items[0];
            if (items.Count == 2) return $"{items[0]} y {items[1]}";
            return string.Join(", ", items.Take(items.Count - 1)) + " y " + items[^1];
        }
    }
}
