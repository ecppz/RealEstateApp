using Application.Dtos.Recommendation;

namespace Application.Recommendation
{
    public sealed class ScoringFunction
    {
        public const decimal WeightPrice = 0.4m;
        public const decimal WeightRooms = 0.2m;
        public const decimal WeightSize = 0.2m;
        public const decimal WeightType = 0.2m;

        public PropertyScoringBreakdown Score(
            RecommendationPropertyDto property,
            UserPreferencesDto preferences)
        {
            var priceActive = preferences.Budget > 0;
            var roomsActive = preferences.Rooms > 0;
            var sizeActive = preferences.Size > 0;
            var typeActive = !string.IsNullOrWhiteSpace(preferences.Type);

            var priceScore = priceActive ? NormalizePrice(property.Price, preferences.Budget) : 0m;
            var roomsScore = roomsActive ? NormalizeRooms(property.Rooms, preferences.Rooms) : 0m;
            var sizeScore = sizeActive ? NormalizeSize(property.Size, preferences.Size) : 0m;
            var typeScore = typeActive ? NormalizeType(property.Type, preferences.Type) : 0m;

            var wPrice = priceActive ? WeightPrice : 0m;
            var wRooms = roomsActive ? WeightRooms : 0m;
            var wSize = sizeActive ? WeightSize : 0m;
            var wType = typeActive ? WeightType : 0m;
            var weightSum = wPrice + wRooms + wSize + wType;

            var weighted =
                (wPrice * priceScore) +
                (wRooms * roomsScore) +
                (wSize * sizeScore) +
                (wType * typeScore);

            var normalizedTotal = weightSum > 0 ? weighted / weightSum : 0m;
            var totalOn100 = Math.Round(normalizedTotal * 100m, 2, MidpointRounding.AwayFromZero);

            return new PropertyScoringBreakdown
            {
                TotalScore = totalOn100,
                PriceScore = priceScore,
                RoomsScore = roomsScore,
                SizeScore = sizeScore,
                TypeScore = typeScore,
                PriceActive = priceActive,
                RoomsActive = roomsActive,
                SizeActive = sizeActive,
                TypeActive = typeActive
            };
        }

        private static decimal NormalizePrice(decimal price, decimal budget)
        {
            if (price <= 0) return 0m;
            if (price <= budget) return 1m;
            var overRatio = (price - budget) / budget;
            if (overRatio >= 1m) return 0m;
            return 1m - overRatio;
        }

        private static decimal NormalizeRooms(int propertyRooms, int preferredRooms)
        {
            var diff = Math.Abs(propertyRooms - preferredRooms);
            var denom = Math.Max(preferredRooms, 1);
            var raw = 1m - (decimal)diff / denom;
            return raw < 0m ? 0m : raw;
        }

        private static decimal NormalizeSize(int propertySize, int preferredSize)
        {
            var diff = Math.Abs(propertySize - preferredSize);
            var denom = Math.Max(preferredSize, 1);
            var raw = 1m - (decimal)diff / denom;
            return raw < 0m ? 0m : raw;
        }

        private static decimal NormalizeType(string? propertyType, string? preferredType)
        {
            if (string.IsNullOrWhiteSpace(propertyType) || string.IsNullOrWhiteSpace(preferredType))
                return 0m;
            return string.Equals(propertyType.Trim(), preferredType.Trim(), StringComparison.OrdinalIgnoreCase)
                ? 1m
                : 0m;
        }
    }
}
