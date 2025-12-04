using Application.Dtos.Property;
using Application.ViewModels.Property;
using AutoMapper;

namespace Application.Mappings.DtosAndViewModels
{
    public class PropertyDtoMappingProfile : Profile
    {
        public PropertyDtoMappingProfile()
        {
            CreateMap<PropertyDto, PropertyViewModel>().ReverseMap();


            CreateMap<PropertyDto, EditPropertyViewModel>().ReverseMap();


            CreateMap<CreatePropertyDto, CreatePropertyViewModel>().ReverseMap();
            
            
            CreateMap<EditPropertyDto, EditPropertyViewModel>().ReverseMap();


            CreateMap<PropertyDto, DeletePropertyViewModel>().ReverseMap();
        }
    }
}
