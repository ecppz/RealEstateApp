using Application.Dtos.Improvement;
using Application.Dtos.PropertyImage;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings.EntitiesAndDtos
{
    public class PropertyImageMappingProfile : Profile
    {
        public PropertyImageMappingProfile()
        {

            CreateMap<PropertyImage, PropertyImageDto>().ReverseMap();

        }
    }
}
