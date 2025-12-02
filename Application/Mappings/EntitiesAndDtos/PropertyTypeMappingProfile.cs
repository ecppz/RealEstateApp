using Application.Dtos.Property;
using Application.Dtos.PropertyType;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings.EntitiesAndDtos
{
    public class PropertyTypeMappingProfile : Profile
    {
        public PropertyTypeMappingProfile()
        {
            // Entidad ↔ DTO general
            CreateMap<PropertyType, PropertyTypeDto>().ReverseMap();

            // CreateDto → Entidad
            CreateMap<PropertyTypeCreateDto, PropertyType>();

            // UpdateDto → Entidad
            CreateMap<PropertyTypeUpdateDto, PropertyType>();

            // Entidad ↔ ListDto
            CreateMap<PropertyType, PropertyTypeListDto>()
                .ForMember(dest => dest.PropertyCount, opt => opt.MapFrom(src => src.Properties != null ? src.Properties.Count : 0))
                .ReverseMap();
        }

    }
}
