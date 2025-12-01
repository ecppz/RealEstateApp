using Application.Dtos.Property;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings.EntitiesAndDtos
{
    public class PropertyMappingProfile : Profile
    {
        public PropertyMappingProfile()
        {
            // Entidad ↔ DTO general
            CreateMap<Property, PropertyDto>()
                .ForMember(dest => dest.PropertyType, opt => opt.MapFrom(src => src.PropertyType))
                .ForMember(dest => dest.SaleType, opt => opt.MapFrom(src => src.SaleType))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
                .ForMember(dest => dest.Improvements, opt => opt.MapFrom(src => src.Improvements))
                .ReverseMap();

            // CreateDto → Entidad
            CreateMap<PropertyCreateDto, Property>();

            // UpdateDto → Entidad
            CreateMap<PropertyUpdateDto, Property>();

            // Entidad ↔ ListDto
            CreateMap<Property, PropertyListDto>()
                .ForMember(dest => dest.PropertyTypeName, opt => opt.MapFrom(src => src.PropertyType != null ? src.PropertyType.Name : string.Empty))
                .ForMember(dest => dest.SaleTypeName, opt => opt.MapFrom(src => src.SaleType != null ? src.SaleType.Name : string.Empty))
                .ReverseMap();
        }
    }
}
