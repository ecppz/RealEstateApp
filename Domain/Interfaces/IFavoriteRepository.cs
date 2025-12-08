using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IFavoriteRepository : IGenericRepository<Favorite>
    {
        Task<List<Favorite>> GetFavoritesByUser(string clientId);

        Task<bool> IsFavorite(string clientId, int propertyId);
    }
}
