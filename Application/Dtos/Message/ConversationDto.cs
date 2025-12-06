using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Message
{
    public class ConversationDto
    {
        public int PropertyId { get; set; }
        public required string AgentId { get; set; }
        public required string ClientId { get; set; }
        public required string ClientName { get; set; }
        public List<MessageDto> Messages { get; set; } = new();
    }

}
