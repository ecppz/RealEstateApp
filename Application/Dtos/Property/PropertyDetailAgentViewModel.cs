using Application.Dtos.Offer;
using Domain.Common.Enums;

namespace Application.Dtos.Property
{
    public class PropertyDetailAgentViewModel
    {
        // Datos básicos de la propiedad
        public required int Id { get; set; }
        public required string Code { get; set; }
        public required decimal Price { get; set; }
        public required string Description { get; set; }
        public required int SizeInMeters { get; set; }
        public required int Bedrooms { get; set; }
        public required int Bathrooms { get; set; }
        public PropertyStatus Status { get; set; }

        // Listado de clientes con mensajes
        public List<ClientSummaryViewModel> ClientsWithMessages { get; set; } = new();

        // Listado de clientes con ofertas
        public List<ClientSummaryViewModel> ClientsWithOffers { get; set; } = new();
    }

}
