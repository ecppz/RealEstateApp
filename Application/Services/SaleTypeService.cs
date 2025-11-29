using Application.Dtos.SaleType;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class SaleTypeService : GenericService<SaleType, SaleTypeDto>, ISaleTypeService
    {
        private readonly ISaleTypeRepository saleTypeRepository;
        private readonly IMapper mapper;

        public SaleTypeService(ISaleTypeRepository saleTypeRepository, IMapper mapper)
            : base(saleTypeRepository, mapper)
        {
            this.saleTypeRepository = saleTypeRepository;
            this.mapper = mapper;
        }
    }
}
