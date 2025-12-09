using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Property
{
    using Application.ViewModels.Message;
    using Application.ViewModels.Offer;
    using Domain.Common.Enums;

    public class PropertyDetailClientViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; } = default!;
        public decimal Price { get; set; }
        public string Description { get; set; } = default!;
        public int SizeInMeters { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public PropertyStatus Status { get; set; }

        // Chat
        public List<MessageViewModel> Messages { get; set; } = new();

        // Ofertas
        public List<OfferViewModel> Offers { get; set; } = new();

        // Agente dueño de la propiedad (para enviar mensajes)
        public string AgentId { get; set; } = default!;
    }
}
