using Application.Dtos.PropertyImprovement;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings.EntitiesAndDtos
{
    public class PropertyImprovementMappingProfile : Profile
    {
        public PropertyImprovementMappingProfile()
        {
            // Entity ↔ DTOs
            CreateMap<PropertyImprovement, PropertyImprovementDto>().ReverseMap();
            CreateMap<PropertyImprovement, PropertyImprovementCreateDto>().ReverseMap();
            CreateMap<PropertyImprovement, PropertyImprovementUpdateDto>().ReverseMap();
            CreateMap<PropertyImprovement, PropertyImprovementListDto>().ReverseMap();
        }
    }
}
