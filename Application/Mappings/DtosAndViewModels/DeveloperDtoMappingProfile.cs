using Application.Dtos.Developer;
using Application.Dtos.User;
using Application.ViewModels.Developer;
using AutoMapper;

namespace Application.Mappings.DtosAndViewModels
{
    public class DeveloperDtoMappingProfile : Profile
    {
        public DeveloperDtoMappingProfile() {

            CreateMap<DeveloperDto, DeveloperViewModel>()
                 .ReverseMap();


            CreateMap<DeveloperDto, EditDeveloperViewModel>()
                 .ReverseMap();


            CreateMap<CreateDeveloperDto, CreateDeveloperViewModel>()
                .ReverseMap();

            CreateMap<EditDeveloperDto, EditDeveloperViewModel>()
                .ReverseMap();


            CreateMap<DeveloperDto, ActivateDeveloperViewModel>().ReverseMap();

            CreateMap<DeveloperDto, DeactivateDeveloperViewModel>().ReverseMap();

            CreateMap<DeveloperDto, SaveUserDto>()
                .ForMember(dest => dest.PhoneNumber, opt => opt.Ignore())
                .ForMember(dest => dest.ProfileImage, opt => opt.Ignore());


            CreateMap<CreateDeveloperDto, SaveUserDto>()
                .ForMember(dest => dest.PhoneNumber, opt => opt.Ignore())
                .ForMember(dest => dest.ProfileImage, opt => opt.Ignore());

            CreateMap<EditDeveloperDto, SaveUserDto>()
                .ForMember(dest => dest.PhoneNumber, opt => opt.Ignore())
                .ForMember(dest => dest.ProfileImage, opt => opt.Ignore());



        }
    }
}
