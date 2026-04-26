using AutoMapper;
using Application.Dtos.User;
using Application.Dtos.Admin;
using Application.Dtos.Agent;
using Infrastructure.Identity.Entities;
using Application.Dtos.Developer;

namespace Infrastructure.Identity.Mappings.EntitiesAndDtos
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserAccount, UserDto>();
            CreateMap<UserAccount, AdminDto>();
            // Este mapa permite que GetAllUsers y GetUserById recuperen la foto
            CreateMap<UserAccount, AgentDto>()
                .ForMember(dest => dest.ProfileImage, opt => opt.MapFrom(src => src.ProfileImage))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ReverseMap();
            CreateMap<UserAccount, DeveloperDto>();
        }
    }
}
