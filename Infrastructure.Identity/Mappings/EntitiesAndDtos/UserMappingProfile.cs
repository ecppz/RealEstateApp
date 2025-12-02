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
            CreateMap<UserAccount, AgentDto>();
            CreateMap<UserAccount, DeveloperDto>();
        }
    }
}
