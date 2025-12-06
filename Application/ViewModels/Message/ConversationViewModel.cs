using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Message
{
    // Hilo completo de conversación entre agente y cliente
    public class ConversationViewModel
    {
        public int PropertyId { get; set; }
        public required string AgentId { get; set; }
        public required string ClientId { get; set; }
        public required string ClientName { get; set; }
        public List<MessageViewModel> Messages { get; set; } = new();
    }

}
