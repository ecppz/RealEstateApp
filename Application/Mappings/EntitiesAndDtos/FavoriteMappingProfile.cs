using Application.Dtos.Favorite;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings.EntitiesAndDtos
{
    public class FavoriteMappingProfile : Profile
    {
        public FavoriteMappingProfile()
        {
            CreateMap<Favorite, FavoriteDto>().ReverseMap();

        }
    }
}
