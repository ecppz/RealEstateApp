using Application.Dtos.Favorite;
using Application.Interfaces;

namespace Application.Services
{
    public interface IFavoriteService : IGenericService<FavoriteDto>
    {
        Task<List<FavoriteDto>> GetFavoritesByUser(string clientId);
        Task<bool> IsFavorite(string clientId, int propertyId);
        Task AddFavorite(string clientId, int propertyId);
        Task RemoveFavorite(string clientId, int propertyId);
    }
}
