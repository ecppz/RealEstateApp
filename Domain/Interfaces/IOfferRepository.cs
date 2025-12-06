using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IOfferRepository : IGenericRepository<Offer>
    {
        // Crear una nueva oferta en estado pendiente
        Task<Offer?> CreateOfferAsync(Offer offer);

        // Obtener todas las ofertas de una propiedad
        Task<List<Offer>> GetOffersByPropertyAsync(int propertyId);

        // Obtener todas las ofertas realizadas por un cliente
        Task<List<Offer>> GetOffersByClientAsync(string clientId);

        // Aceptar una oferta específica y rechazar las demás pendientes de la misma propiedad
        Task<bool> AcceptOfferAsync(int offerId);

        // Rechazar una oferta específica
        Task<bool> RejectOfferAsync(int offerId);

        Task<Offer?> GetOfferByIdAsync(int offerId);

    }
}
