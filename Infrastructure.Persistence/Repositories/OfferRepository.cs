using Domain.Common.Enums;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class OfferRepository : GenericRepository<Offer>, IOfferRepository
    {
        public OfferRepository(RealEstateAppContext context) : base(context) { }

        // Crear una nueva oferta en estado pendiente
        public async Task<Offer?> CreateOfferAsync(Offer offer)
        {
            offer.Date = DateTime.UtcNow;
            offer.Status = OfferStatus.Pending;

            await context.Offers.AddAsync(offer);
            await context.SaveChangesAsync();
            return offer;
        }

        public async Task<Offer?> GetOfferByIdAsync(int offerId)
        {
            return await context.Offers
                .Include(o => o.Property) // opcional, si quieres la propiedad asociada
                .FirstOrDefaultAsync(o => o.Id == offerId);
        }

        // Obtener todas las ofertas de una propiedad
        public async Task<List<Offer>> GetOffersByPropertyAsync(int propertyId)
        {
            return await context.Offers
                .Where(o => o.PropertyId == propertyId)
                .OrderByDescending(o => o.Date)
                .ToListAsync();
        }

        // Obtener todas las ofertas realizadas por un cliente
        public async Task<List<Offer>> GetOffersByClientAsync(string clientId)
        {
            return await context.Offers
                .Where(o => o.ClientId == clientId)
                .OrderByDescending(o => o.Date)
                .ToListAsync();
        }

        public async Task<bool> AcceptOfferAsync(int offerId)
        {
            var offer = await context.Offers.FindAsync(offerId);
            if (offer == null) return false;

            // Marcar la oferta como aceptada
            offer.Status = OfferStatus.Accepted;

            // Rechazar todas las demás pendientes de la misma propiedad
            var pendingOffers = await context.Offers
                .Where(o => o.PropertyId == offer.PropertyId && o.Id != offerId && o.Status == OfferStatus.Pending)
                .ToListAsync();

            foreach (var pending in pendingOffers)
            {
                pending.Status = OfferStatus.Rejected;
            }

            // Marcar la propiedad como vendida
            var property = await context.Properties.FindAsync(offer.PropertyId);
            if (property != null)
            {
                property.Status = PropertyStatus.Sold;
            }

            await context.SaveChangesAsync();
            return true;
        }

        // Rechazar una oferta específica
        public async Task<bool> RejectOfferAsync(int offerId)
        {
            var offer = await context.Offers.FindAsync(offerId);
            if (offer == null) return false;

            offer.Status = OfferStatus.Rejected;
            await context.SaveChangesAsync();
            return true;
        }

    }
}
