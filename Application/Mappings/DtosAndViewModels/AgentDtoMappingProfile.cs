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
               .ForMember(dest => dest.PhoneNumber, opt => opt.Ignore());

            CreateMap<AgentDto, DeactivateAgentViewModel>()
                .ReverseMap()
                .ForMember(dest => dest.ProfileImage, opt => opt.Ignore())
                .ForMember(dest => dest.PhoneNumber, opt => opt.Ignore());

            CreateMap<AgentDto, DeleteAgentViewModel>()
                .ReverseMap()
                .ForMember(dest => dest.ProfileImage, opt => opt.Ignore())
                .ForMember(dest => dest.PhoneNumber, opt => opt.Ignore());

            CreateMap<AgentDto, AgentProfileViewModel>()
                .ReverseMap();

            CreateMap<AgentDto, SaveUserDto>()
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.ProfileImage, opt => opt.MapFrom(src => src.ProfileImage)) 
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role));


            CreateMap<EditAgentDto, SaveUserDto>()
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.ProfileImage, opt => opt.Ignore());

            CreateMap<CreateAgentDto, SaveUserDto>()
             .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
             .ForMember(dest => dest.ProfileImage, opt => opt.MapFrom(src => src.ProfileImage))
             .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
             .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
             .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
             .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
             .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
             .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
             .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));


        }
    }
}
