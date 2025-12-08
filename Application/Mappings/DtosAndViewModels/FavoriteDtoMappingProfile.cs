using Application.Dtos.Favorite;
using Application.ViewModels.Favorite;
using AutoMapper;

namespace Application.Mappings.DtosAndViewModels
{
    public class FavoriteDtoMappingProfile : Profile
    {
        public FavoriteDtoMappingProfile()
        {
            CreateMap<FavoriteDto, FavoriteViewModel>().ReverseMap();

        }
    }
}
