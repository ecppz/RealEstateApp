using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        public MessageRepository(RealEstateAppContext context) : base(context) { }

        //  Obtener todos los clientes que han iniciado conversación sobre una propiedad
        public async Task<List<string>> GetClientsByPropertyAsync(int propertyId)
        {
            return await context.Set<Message>()
                .Where(m => m.PropertyId == propertyId)
                .Select(m => m.SenderId)
                .Distinct()
                .ToListAsync();
        }

        //  Obtener todos los mensajes entre un agente y un cliente para una propiedad específica
        public async Task<List<Message>> GetConversationAsync(int propertyId, string agentId, string clientId)
        {
            return await context.Set<Message>()
                .Where(m => m.PropertyId == propertyId &&
                           ((m.SenderId == agentId && m.ReceiverId == clientId) ||
                            (m.SenderId == clientId && m.ReceiverId == agentId)))
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }

        //  Obtener todos los mensajes enviados por un agente en una propiedad
        public async Task<List<Message>> GetMessagesByAgentAsync(int propertyId, string agentId)
        {
            return await context.Set<Message>()
                .Where(m => m.PropertyId == propertyId && m.SenderId == agentId)
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }

        //  Obtener todos los mensajes enviados por un cliente en una propiedad
        public async Task<List<Message>> GetMessagesByClientAsync(int propertyId, string clientId)
        {
            return await context.Set<Message>()
                .Where(m => m.PropertyId == propertyId && m.SenderId == clientId)
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }

        // Guardar un nuevo mensaje en la conversación
        public async Task<Message?> SendMessageAsync(Message message)
        {
            await context.Set<Message>().AddAsync(message);
            await context.SaveChangesAsync();
            return message;
        }


    }
}
