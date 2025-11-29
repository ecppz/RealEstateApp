using Application.Dtos.Message;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class MessageService : GenericService<Message, MessageDto>, IMessageService
    {
        private readonly IMessageRepository messageRepository;
        private readonly IMapper mapper;

        public MessageService(IMessageRepository messageRepository, IMapper mapper)
            : base(messageRepository, mapper)
        {
            this.messageRepository = messageRepository;
            this.mapper = mapper;
        }
    }
}
