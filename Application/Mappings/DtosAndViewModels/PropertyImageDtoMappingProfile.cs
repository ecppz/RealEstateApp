using Application.Dtos.PropertyImage;
using Application.ViewModels.PropertyImage;
using AutoMapper;

namespace Application.Mappings.EntitiesAndDtos
{
    public class PropertyImageDtoMappingProfile : Profile
    {
        public PropertyImageDtoMappingProfile()
        {

            CreateMap<PropertyImageDto, PropertyImageViewModel>().ReverseMap();

        }
    }
}
