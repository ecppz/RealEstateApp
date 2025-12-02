using Application.Dtos.Admin;
using Application.Dtos.User;
using Application.ViewModels.Admin;
using AutoMapper;

namespace Application.Mappings.DtosAndViewModels
{
    public class AdminDtoMappingProfile : Profile
    {
        public AdminDtoMappingProfile() {

            CreateMap<AdminDto, AdminViewModel>()
                 .ReverseMap();


            CreateMap<AdminDto, EditAdminViewModel>()
                 .ReverseMap();


            CreateMap<CreateAdminDto, CreateAdminViewModel>()
                .ReverseMap();

            CreateMap<EditAdminDto, EditAdminViewModel>()
                .ReverseMap();


            CreateMap<AdminDto, ActivateAdminViewModel>().ReverseMap();

            CreateMap<AdminDto, DeactivateAdminViewModel>().ReverseMap();

            CreateMap<AdminDto, SaveUserDto>()
                .ForMember(dest => dest.PhoneNumber, opt => opt.Ignore())
                .ForMember(dest => dest.ProfileImage, opt => opt.Ignore());


            CreateMap<CreateAdminDto, SaveUserDto>()
                .ForMember(dest => dest.PhoneNumber, opt => opt.Ignore())
                .ForMember(dest => dest.ProfileImage, opt => opt.Ignore());

            CreateMap<EditAdminDto, SaveUserDto>()
                .ForMember(dest => dest.PhoneNumber, opt => opt.Ignore())
                .ForMember(dest => dest.ProfileImage, opt => opt.Ignore());



        }
    }
}
