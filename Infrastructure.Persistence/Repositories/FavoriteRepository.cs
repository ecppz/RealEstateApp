using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class FavoriteRepository : GenericRepository<Favorite>, IFavoriteRepository
    {
        public FavoriteRepository(RealEstateAppContext context) : base(context) { }

        public async Task<List<Favorite>> GetFavoritesByUser(string clientId)
        {
            return await context.Favorites
                .Where(f => f.ClientId == clientId)
                .Include(f => f.Properties)
                    .ThenInclude(p => p.Images)
                .Include(f => f.Properties)
                    .ThenInclude(p => p.Improvements)
                        .ThenInclude(pi => pi.Improvement)
                .Include(f => f.Properties)
                    .ThenInclude(p => p.PropertyType)
                .Include(f => f.Properties)
                    .ThenInclude(p => p.SaleType)
                .ToListAsync();
        }


        public async Task<bool> IsFavorite(string clientId, int propertyId)
        {
            return await context.Favorites
                .AnyAsync(f => f.ClientId == clientId && f.PropertyId == propertyId);
        }
    }
}
