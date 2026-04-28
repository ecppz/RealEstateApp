namespace Application.Dtos.Recommendation
{
    public sealed class PropertyScoringBreakdown
    {
        public decimal TotalScore { get; init; }

        public decimal PriceScore { get; init; }
        public decimal RoomsScore { get; init; }
        public decimal SizeScore { get; init; }
        public decimal TypeScore { get; init; }

        public bool PriceActive { get; init; }
        public bool RoomsActive { get; init; }
        public bool SizeActive { get; init; }
        public bool TypeActive { get; init; }
    }
}
