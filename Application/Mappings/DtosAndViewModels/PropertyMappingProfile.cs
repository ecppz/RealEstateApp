using Application.Dtos.Property;
using Application.ViewModels.Property;
using AutoMapper;

namespace Application.Mappings.DtosAndViewModels
{
    public class PropertyMappingProfile : Profile
    {
        public PropertyMappingProfile()
        {
            // DTO ↔ ViewModels
            CreateMap<PropertyDto, PropertyListViewModel>()
                .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.PropertyType != null ? src.PropertyType.Name : string.Empty))
                .ReverseMap();

            CreateMap<PropertyCreateViewModel, PropertyDto>().ReverseMap();
            CreateMap<PropertyEditViewModel, PropertyDto>().ReverseMap();
        }
    }
}
