using Application.Dtos.Message;
using Application.Interfaces;

namespace Application.Services
{
    public interface IMessageService : IGenericService<MessageDto>
    {
        //  Listar todos los clientes que han iniciado conversación sobre una propiedad
        Task<List<ChatClientDto>> GetClientsByPropertyAsync(int propertyId, string agentId);

        //  Obtener el hilo completo de conversación entre un agente y un cliente
        Task<ConversationDto?> GetConversationAsync(int propertyId, string agentId, string clientId);

        //  Obtener todos los mensajes enviados por un agente en una propiedad
        Task<List<MessageDto>> GetMessagesByAgentAsync(int propertyId, string agentId);

       //  Obtener todos los mensajes enviados por un cliente en una propiedad
        Task<List<MessageDto>> GetMessagesByClientAsync(int propertyId, string clientId);

        //  Enviar un nuevo mensaje en la conversación
        Task<MessageDto?> SendMessageAsync(SendMessageDto dto);

    }
}
