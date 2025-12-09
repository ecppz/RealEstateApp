using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IMessageRepository : IGenericRepository<Message>
    {
        // Obtener todos los clientes que han iniciado conversación sobre una propiedad
        Task<List<string>> GetClientsByPropertyAsync(int propertyId);

        // Obtener todos los mensajes entre un agente y un cliente para una propiedad específica
        Task<List<Message>> GetConversationAsync(int propertyId, string agentId, string clientId);

        // Obtener todos los mensajes enviados por un agente en una propiedad
        Task<List<Message>> GetMessagesByAgentAsync(int propertyId, string agentId);

        // Obtener todos los mensajes enviados por un cliente en una propiedad
        Task<List<Message>> GetMessagesByClientAsync(int propertyId, string clientId);

        // Guardar un nuevo mensaje en la conversación
        Task<Message?> SendMessageAsync(Message message);

    }
}
