using Application.Dtos.Offer;
using Application.Dtos.Property;
using Application.Interfaces;
using AutoMapper;
using Domain.Common.Enums;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class OfferService : GenericService<Offer, OfferDto>, IOfferService
    {
        private readonly IOfferRepository offerRepository;
        private readonly IMapper mapper;
        private readonly IMessageRepository messageRepository;
        private readonly IPropertyRepository propertyRepository;

        public OfferService(IOfferRepository offerRepository,IMapper mapper, IMessageRepository messageRepository,IPropertyRepository propertyRepository)
            : base(offerRepository, mapper)
        {
            this.offerRepository = offerRepository;
            this.mapper = mapper;
            this.propertyRepository = propertyRepository;
            this.messageRepository = messageRepository;
        }

        // Crear una nueva oferta (cliente)
        public async Task<OfferDto?> CreateOfferAsync(CreateOfferDto dto)
        {
            var offer = new Offer
            {
                PropertyId = dto.PropertyId,
                ClientId = dto.ClientId,
                Amount = dto.Amount,
                Date = DateTime.UtcNow,
                Status = OfferStatus.Pending
            };

            var created = await offerRepository.CreateOfferAsync(offer);

            // Mapeo manual sensible → OfferDto
            return created == null ? null : new OfferDto
            {
                Id = created.Id,
                PropertyId = created.PropertyId,
                ClientId = created.ClientId,
                Amount = created.Amount,
                Date = created.Date,
                Status = created.Status,
                Property = mapper.Map<PropertyDto>(created.Property)
            };
        }

        // Obtener todas las ofertas de una propiedad (agente)
        public async Task<List<OfferDto>> GetOffersByPropertyAsync(int propertyId)
        {
            var offers = await offerRepository.GetOffersByPropertyAsync(propertyId);
            return mapper.Map<List<OfferDto>>(offers);
        }

        // Obtener todas las ofertas realizadas por un cliente (cliente)
        public async Task<List<OfferDto>> GetOffersByClientAsync(string clientId)
        {
            var offers = await offerRepository.GetOffersByClientAsync(clientId);
            return mapper.Map<List<OfferDto>>(offers);
        }

        //Comento aqui por si aca
        public async Task<bool> AcceptOfferAsync(int offerId)
        {
            var offer = await offerRepository.GetOfferByIdAsync(offerId);
            if (offer == null || offer.Status != OfferStatus.Pending)
                return false;

            var property = await propertyRepository.GetPropertyByIdAsync(offer.PropertyId);
            if (property == null || property.Status == PropertyStatus.Sold)
                return false;

            // Aceptar oferta
            var accepted = await offerRepository.AcceptOfferAsync(offerId);
            if (!accepted) return false;

            // Marcar propiedad como vendida
            property.Status = PropertyStatus.Sold;
            await propertyRepository.UpdateAsync(property.Id, property);

            // Rechazar las demás pendientes
            var allOffers = await offerRepository.GetOffersByPropertyAsync(offer.PropertyId);
            foreach (var pending in allOffers.Where(o => o.Id != offerId && o.Status == OfferStatus.Pending))
            {
                await offerRepository.RejectOfferAsync(pending.Id);
            }

            return true;
        }

        public async Task<bool> RejectOfferAsync(int offerId)
        {
            var offer = await offerRepository.GetOfferByIdAsync(offerId);
            if (offer == null || offer.Status != OfferStatus.Pending)
                return false;

            return await offerRepository.RejectOfferAsync(offerId);
        }

    }
}
