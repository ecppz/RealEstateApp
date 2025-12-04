using Application.Dtos.Improvement;
using Application.ViewModels.Improvement;
using AutoMapper;

namespace Application.Mappings.DtosAndViewModels
{
    public class ImprovementViewModelMappingProfile : Profile
    {
        public ImprovementViewModelMappingProfile()
        {
            // General
            // General
            CreateMap<ImprovementViewModel, ImprovementDto>().ReverseMap();

            // Create
            CreateMap<ImprovementCreateViewModel, ImprovementCreateDto>().ReverseMap();

            // Edit
            CreateMap<ImprovementEditViewModel, ImprovementUpdateDto>().ReverseMap();
            CreateMap<ImprovementEditViewModel, ImprovementDto>().ReverseMap();


            // Delete
            CreateMap<ImprovementDeleteViewModel, ImprovementDeleteDto>().ReverseMap();
            CreateMap<ImprovementDeleteViewModel, ImprovementDto>().ReverseMap();
        }
    }
}
