using Application.Dtos.PropertyImprovement;
using Application.ViewModels.PropertyImprovement;
using AutoMapper;

namespace Application.Mappings.DtosAndViewModels
{
    public class PropertyImprovementViewModelMappingProfile : Profile
    {
        public PropertyImprovementViewModelMappingProfile()
        {
            // General
            CreateMap<PropertyImprovementViewModel, PropertyImprovementDto>().ReverseMap();

            // Create
            CreateMap<PropertyImprovementCreateViewModel, PropertyImprovementCreateDto>().ReverseMap();

            // Edit
            CreateMap<PropertyImprovementEditViewModel, PropertyImprovementUpdateDto>().ReverseMap();

            // List
            CreateMap<PropertyImprovementListViewModel, PropertyImprovementListDto>().ReverseMap();
        }
    }
}
