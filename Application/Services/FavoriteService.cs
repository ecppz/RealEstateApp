using Application.Dtos.Favorite;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class FavoriteService : GenericService<Favorite, FavoriteDto>, IFavoriteService
    {
        private readonly IFavoriteRepository favoriteRepository;
        private readonly IMapper mapper;

        public FavoriteService(IFavoriteRepository favoriteRepository, IMapper mapper)
            : base(favoriteRepository, mapper)
        {
            this.favoriteRepository = favoriteRepository;
            this.mapper = mapper;
        }

        public async Task<List<FavoriteDto>> GetFavoritesByUser(string clientId)
        {
            var favorites = await favoriteRepository.GetFavoritesByUser(clientId);
            return mapper.Map<List<FavoriteDto>>(favorites);
        }

        public async Task<bool> IsFavorite(string clientId, int propertyId)
        {
            return await favoriteRepository.IsFavorite(clientId, propertyId);
        }



        public async Task AddFavorite(string clientId, int propertyId)
        {
            // Evitar duplicados
            var exists = await favoriteRepository.IsFavorite(clientId, propertyId);
            if (!exists)
            {
                var favorite = new Favorite
                {
                    Id = 0,
                    ClientId = clientId,
                    PropertyId = propertyId
                };
                await favoriteRepository.AddAsync(favorite);
            }
        }

        public async Task RemoveFavorite(string clientId, int propertyId)
        {
            var favorites = await favoriteRepository.GetFavoritesByUser(clientId);
            var fav = favorites.FirstOrDefault(f => f.PropertyId == propertyId);
            if (fav != null)
            {
                await favoriteRepository.DeleteAsync(fav.Id);
            }
        }

    }
}
