using Application.Dtos.PropertyType;
using Application.ViewModels.PropertyType;
using AutoMapper;

namespace Application.Mappings.DtosAndViewModels
{
    public class PropertyTypeMappingProfile : Profile
    {
        public PropertyTypeMappingProfile()
        {
            // DTO ↔ ViewModels
            CreateMap<PropertyTypeDto, PropertyTypeListViewModel>()
                .ForMember(dest => dest.PropertyCount, opt => opt.MapFrom(src => src.Properties != null ? src.Properties.Count : 0))
                .ReverseMap();

            CreateMap<PropertyTypeListDto, PropertyTypeListViewModel>().ReverseMap();

            CreateMap<PropertyTypeCreateViewModel, PropertyTypeCreateDto>().ReverseMap();
            CreateMap<PropertyTypeEditViewModel, PropertyTypeUpdateDto>().ReverseMap();

            CreateMap<PropertyTypeDto, PropertyTypeEditViewModel>().ReverseMap();
        }
    }
}
