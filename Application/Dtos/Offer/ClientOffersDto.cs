using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Offer
{
    // Listado de ofertas por cliente (para cliente)
    public class ClientOffersDto
    {
        public required int ClientId { get; set; }
        public List<OfferDto> Offers { get; set; } = new();
    }

}
