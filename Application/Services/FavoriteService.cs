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
    }
}
