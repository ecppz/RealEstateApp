using Application.Dtos.Agent;
using Application.Dtos.User;
using Application.ViewModels.Agent;
using AutoMapper;

namespace Application.Mappings.DtosAndViewModels
{
    public class AgentDtoMappingProfile : Profile
    {
        public AgentDtoMappingProfile() {


            CreateMap<AgentDto, AgentViewModel>()
                 .ReverseMap();

            CreateMap<AgentDto, ActivateAgentViewModel>()
               .ReverseMap()
               .ForMember(dest => dest.ProfileImage, opt => opt.Ignore())
               .ForMember(dest => dest.Phone, opt => opt.Ignore());

            CreateMap<AgentDto, DeactivateAgentViewModel>()
                .ReverseMap()
                .ForMember(dest => dest.ProfileImage, opt => opt.Ignore())
                .ForMember(dest => dest.Phone, opt => opt.Ignore());

            CreateMap<AgentDto, DeleteAgentViewModel>()
                .ReverseMap();

            CreateMap<AgentDto, SaveUserDto>()
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.Phone, opt => opt.Ignore())
                .ForMember(dest => dest.ProfileImage, opt => opt.Ignore())
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));


        }
    }
}
