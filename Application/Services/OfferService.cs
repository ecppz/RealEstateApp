using Application.Dtos.Offer;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class OfferService : GenericService<Offer, OfferDto>, IOfferService
    {
        private readonly IOfferRepository offerRepository;
        private readonly IMapper mapper;

        public OfferService(IOfferRepository offerRepository, IMapper mapper)
            : base(offerRepository, mapper)
        {
            this.offerRepository = offerRepository;
            this.mapper = mapper;
        }
    }
}
