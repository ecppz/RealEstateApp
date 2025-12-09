using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Message
{
    public class ChatClientViewModel
    {
        public required string ClientId { get; set; }
        public required string ClientName { get; set; }
        public int PropertyId { get; set; }
        public DateTime LastMessageAt { get; set; }
    }

}
