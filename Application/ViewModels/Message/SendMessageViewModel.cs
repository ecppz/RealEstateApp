using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Message
{
    // Formulario para enviar un nuevo mensaje desde la vista
    public class SendMessageViewModel
    {
        public int PropertyId { get; set; }
        public required string ReceiverId { get; set; }
        public required string Content { get; set; }
    }

}
