using Application.Dtos.Message;
using Application.Dtos.User;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappings.EntitiesAndDtos
{
    public class MessageMappingProfile : Profile
    {
        public MessageMappingProfile()
        {
            // Entidad ↔ DTO principal
            CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.Property, opt => opt.MapFrom(src => src.Property))
                .ReverseMap();

            //  DTO para enviar mensajes (solo hacia entidad)
            CreateMap<SendMessageDto, Message>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.SentAt, opt => opt.Ignore()) // se setea en el servicio
                .ForMember(dest => dest.Property, opt => opt.Ignore());

            // 🔹 Conversación: se mapea manualmente en el servicio, pero dejamos base
            CreateMap<Message, ConversationDto>()
                .ForMember(dest => dest.Messages, opt => opt.Ignore()); // se llena en el servicio

            //  Cliente del chat: se arma manualmente, no requiere mapeo directo
            // pero dejamos la base por si se quiere mapear desde UserDto
            CreateMap<UserDto, ChatClientDto>()
                .ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.PropertyId, opt => opt.Ignore())
                .ForMember(dest => dest.LastMessageAt, opt => opt.Ignore());
        }
    }

}
