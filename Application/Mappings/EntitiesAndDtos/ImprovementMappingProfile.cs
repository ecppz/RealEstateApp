using Application.Dtos.Improvement;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings.EntitiesAndDtos
{
    public class ImprovementMappingProfile : Profile
    {
        public ImprovementMappingProfile()
        {
            // Entity ↔ DTOs
            CreateMap<Improvement, ImprovementDto>().ReverseMap();
            CreateMap<Improvement, ImprovementCreateDto>().ReverseMap();
            CreateMap<Improvement, ImprovementUpdateDto>().ReverseMap();
        }
    }
}
