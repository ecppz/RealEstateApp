using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Message
{
    // Para listar los clientes que han conversado sobre una propiedad
    public class ChatClientDto
    {
        public required string ClientId { get; set; }
        public required string ClientName { get; set; }
        public int PropertyId { get; set; }
        public DateTime LastMessageAt { get; set; } // útil para ordenar por actividad reciente
    }

}
