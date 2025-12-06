using Application.Dtos.Message;
using Application.ViewModels.Message;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappings.DtosAndViewModels
{
    public class MessageMappingProfile : Profile
    {
        public MessageMappingProfile()
        {


            CreateMap<MessageDto, MessageViewModel>()
             .ForMember(dest => dest.Property, opt => opt.MapFrom(src => src.Property))
             .ForMember(dest => dest.SentAt, opt => opt.MapFrom(src =>
                 TimeZoneInfo.ConvertTimeFromUtc(
                     DateTime.SpecifyKind(src.SentAt, DateTimeKind.Utc),
                     TimeZoneInfo.FindSystemTimeZoneById("SA Western Standard Time")
                 )
             ))
             .ReverseMap();

            CreateMap<ChatClientDto, ChatClientViewModel>().ReverseMap();

            CreateMap<ConversationDto, ConversationViewModel>()
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => src.Messages))
                .ReverseMap();

            CreateMap<SendMessageDto, SendMessageViewModel>().ReverseMap();
        }
    }

}
