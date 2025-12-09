using Application.Dtos.Offer;
using Application.Interfaces;

namespace Application.Services
{
    public interface IOfferService : IGenericService<OfferDto>
    {
        // Crear una nueva oferta en estado pendiente
        Task<OfferDto?> CreateOfferAsync(CreateOfferDto dto);

        // Obtener todas las ofertas de una propiedad (vista del agente)
        Task<List<OfferDto>> GetOffersByPropertyAsync(int propertyId);

        // Obtener todas las ofertas realizadas por un cliente (vista del cliente)
        Task<List<OfferDto>> GetOffersByClientAsync(string clientId);

        // Aceptar una oferta y rechazar las demás pendientes de la misma propiedad
        Task<bool> AcceptOfferAsync(int offerId);

        // Rechazar una oferta específica
        Task<bool> RejectOfferAsync(int offerId);
    }
}
