using Application.Dtos.Property;
using Application.Dtos.PropertyImprovement;
using Application.Dtos.SaleType;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SaleType.Queries.GetById
{
    public class GetSaleTypeByIdQuery : IRequest<SaleTypeListDto?>
    {
        public int Id { get; set; }
    }

    public class GetSaleTypeByIdQueryHandler : IRequestHandler<GetSaleTypeByIdQuery, SaleTypeListDto?>
    {
        private readonly ISaleTypeRepository _saleTypeRepository;

        public GetSaleTypeByIdQueryHandler(ISaleTypeRepository saleTypeRepository)
        {
            _saleTypeRepository = saleTypeRepository;
        }

        public async Task<SaleTypeListDto?> Handle(GetSaleTypeByIdQuery query, CancellationToken cancellationToken)
        {
            var saleType = await _saleTypeRepository.GetById(query.Id);

            if (saleType == null)
            {
                return null; 
            }

            return new SaleTypeListDto
            {
                Id = saleType.Id,
                Name = saleType.Name,
                Description = saleType.Description,
                PropertyCount = saleType.Properties?.Count ?? 0 
            };
        }
    }


}
