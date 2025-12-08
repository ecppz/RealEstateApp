using Application.Dtos.Property;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings.EntitiesAndDtos
{
    public class PropertyMappingProfile : Profile
    {
        public PropertyMappingProfile()
        {
            CreateMap<Property, PropertyDto>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
                .ForMember(dest => dest.Improvements, opt => opt.MapFrom(src => src.Improvements))
                .ReverseMap();

            CreateMap<Property, CreatePropertyDto>().ReverseMap();

            CreateMap<Property, EditPropertyDto>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
                .ForMember(dest => dest.Improvements, opt => opt.MapFrom(src => src.Improvements))
                .ReverseMap();

            CreateMap<Property, EditPropertyDto>().ReverseMap();

        }
    }
}
