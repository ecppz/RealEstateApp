using Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Offer
{
    // Respuesta del agente (aceptar/rechazar)
    public class UpdateOfferStatusDto
    {
        public required int OfferId { get; set; }
        public required OfferStatus Status { get; set; }
    }

}
