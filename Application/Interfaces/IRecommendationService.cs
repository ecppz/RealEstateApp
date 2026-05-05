using Application.Dtos.Recommendation;

namespace Application.Interfaces
{
    public interface IRecommendationService
    {
        RecommendResponseDto Recommend(RecommendRequestDto request);
    }
}
