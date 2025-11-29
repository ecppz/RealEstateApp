using Application.Dtos.Improvement;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class ImprovementService : GenericService<Improvement, ImprovementDto>, IImprovementService
    {
        private readonly IImprovementRepository improvementRepository;
        private readonly IMapper mapper;

        public ImprovementService(IImprovementRepository improvementRepository, IMapper mapper)
            : base(improvementRepository, mapper)
        {
            this.improvementRepository = improvementRepository;
            this.mapper = mapper;
        }



    }
}
