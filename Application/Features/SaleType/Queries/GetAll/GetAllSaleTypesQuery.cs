using Application.Dtos.SaleType;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SaleType.Queries.GetAll
{
    public class GetAllSaleTypesQuery : IRequest<List<SaleTypeListDto>>
    {
    }

    public class GetAllSaleTypesQueryHandler : IRequestHandler<GetAllSaleTypesQuery, List<SaleTypeListDto>>
    {
        private readonly ISaleTypeRepository _saleTypeRepository;

        public GetAllSaleTypesQueryHandler(ISaleTypeRepository saleTypeRepository)
        {
            _saleTypeRepository = saleTypeRepository;
        }

        public async Task<List<SaleTypeListDto>> Handle(GetAllSaleTypesQuery query, CancellationToken cancellationToken)
        {
            var saleTypes = await _saleTypeRepository.GetAllList();

            if (saleTypes == null || !saleTypes.Any())
            {
                return new List<SaleTypeListDto>();
            }

            return saleTypes.Select(st => new SaleTypeListDto
            {
                Id = st.Id,
                Name = st.Name,
                Description = st.Description,
                PropertyCount = st.Properties?.Count ?? 0
            }).ToList();
        }
    }

}
