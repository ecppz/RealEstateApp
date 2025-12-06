using Application.Dtos.Property;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings.EntitiesAndDtos
{
    public class PropertyMappingProfile : Profile
    {
        public PropertyMappingProfile()
        {
            // 🔹 Entidad → DTO principal
            CreateMap<Property, PropertyDto>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
                .ForMember(dest => dest.Improvements, opt => opt.MapFrom(src => src.Improvements))
                .ReverseMap();

            // 🔹 Entidad → Create DTO
            CreateMap<Property, CreatePropertyDto>()
                .ForMember(dest => dest.Images, opt => opt.Ignore()) // se ignoran porque se suben aparte
                .ReverseMap();

            // 🔹 Entidad → Edit DTO
            CreateMap<Property, EditPropertyDto>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
                .ForMember(dest => dest.Improvements, opt => opt.MapFrom(src => src.Improvements))
                .ReverseMap();

            // 🔹 Entidad → ViewModel para detalle de cliente
            CreateMap<Property, PropertyDetailClientViewModel>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.SizeInMeters, opt => opt.MapFrom(src => src.SizeInMeters))
                .ForMember(dest => dest.Bedrooms, opt => opt.MapFrom(src => src.Bedrooms))
                .ForMember(dest => dest.Bathrooms, opt => opt.MapFrom(src => src.Bathrooms))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.AgentId, opt => opt.MapFrom(src => src.AgentId))
                .ReverseMap();
        }
    }
}
