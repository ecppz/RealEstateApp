using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Offer
{
    // Crear nueva oferta (cliente)
    public class CreateOfferDto
    {
        public required int PropertyId { get; set; }
        public required string ClientId { get; set; }
        public required decimal Amount { get; set; }
    }

}
