using Application.Dtos.Message;
using Application.Dtos.User;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class MessageService : GenericService<Message, MessageDto>, IMessageService
    {
        private readonly IMessageRepository messageRepository;
        private readonly IMapper mapper;
        private readonly IPropertyRepository propertyRepository;
      //  private readonly IUserAccountServiceForWebApp usuarioService;

        public MessageService(IMessageRepository messageRepository, IPropertyRepository propertyRepository, 
            //IUserAccountServiceForWebApp baseAccountService,
            IMapper mapper)
            : base(messageRepository, mapper)
        {
            this.messageRepository = messageRepository;
            this.mapper = mapper;
            this.propertyRepository = propertyRepository;
        //    this.usuarioService = baseAccountService;
        }


        //  Listar todos los clientes que han iniciado conversación sobre una propiedad
        //public async Task<List<ChatClientDto>> GetClientsByPropertyAsync(int propertyId, string agentId)
        //{
        //    var clientIds = await messageRepository.GetClientsByPropertyAsync(propertyId);
        //    var result = new List<ChatClientDto>();

        //    foreach (var clientId in clientIds.Where(id => id != agentId)) // 🔹 excluir agente
        //    {
        //        var user = await usuarioService.GetUserById<UserDto>(clientId);
        //        var lastMessage = (await messageRepository.GetMessagesByClientAsync(propertyId, clientId))
        //                            .OrderByDescending(m => m.SentAt)
        //                            .FirstOrDefault();

        //        result.Add(new ChatClientDto
        //        {
        //            ClientId = clientId,
        //            ClientName = user?.UserName ?? "Cliente desconocido",
        //            PropertyId = propertyId,
        //            LastMessageAt = lastMessage?.SentAt ?? DateTime.MinValue
        //        });
        //    }

        //    return result.OrderByDescending(c => c.LastMessageAt).ToList();
        //}

        //  Obtener el hilo completo de conversación entre un agente y un cliente
        //public async Task<ConversationDto?> GetConversationAsync(int propertyId, string agentId, string clientId)
        //{
        //    var messages = await messageRepository.GetConversationAsync(propertyId, agentId, clientId);
        //    var mappedMessages = mapper.Map<List<MessageDto>>(messages);

        //    var client = await usuarioService.GetUserById<UserDto>(clientId);

        //    return new ConversationDto
        //    {
        //        PropertyId = propertyId,
        //        AgentId = agentId,
        //        ClientId = clientId,
        //        ClientName = client?.UserName ?? "Cliente desconocido",
        //        Messages = mappedMessages
        //    };
        //}

        // Obtener todos los mensajes enviados por un agente en una propiedad
        public async Task<List<MessageDto>> GetMessagesByAgentAsync(int propertyId, string agentId)
        {
            var messages = await messageRepository.GetMessagesByAgentAsync(propertyId, agentId);
            return mapper.Map<List<MessageDto>>(messages);
        }

        //  Obtener todos los mensajes enviados por un cliente en una propiedad
        public async Task<List<MessageDto>> GetMessagesByClientAsync(int propertyId, string clientId)
        {
            var messages = await messageRepository.GetMessagesByClientAsync(propertyId, clientId);
            return mapper.Map<List<MessageDto>>(messages);
        }

        //  Enviar un nuevo mensaje en la conversación
        public async Task<MessageDto?> SendMessageAsync(SendMessageDto dto)
        {
           // asegurar que la propiedad existe y está activa
            var property = await propertyRepository.GetPropertyByIdAsync(dto.PropertyId);
            if (property == null)
                throw new Exception("La propiedad no existe.");

            var message = new Message
            {
                PropertyId = dto.PropertyId,
                SenderId = dto.SenderId,
                ReceiverId = dto.ReceiverId,
                Content = dto.Content,
                SentAt = DateTime.Now
            };

            var saved = await messageRepository.SendMessageAsync(message);
            return mapper.Map<MessageDto>(saved);
        }




    }
}
