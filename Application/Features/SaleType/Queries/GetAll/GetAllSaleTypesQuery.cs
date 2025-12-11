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
    /// <summary>
    /// Query to retrieve all sale types with their details and property count.
    /// </summary>
    public class GetAllSaleTypesQuery : IRequest<List<SaleTypeListDto>>
    {
        // No parameters required for this query
    }

    /// <summary>
    /// Handles the retrieval of all sale types.
    /// </summary>
    public class GetAllSaleTypesQueryHandler : IRequestHandler<GetAllSaleTypesQuery, List<SaleTypeListDto>>
    {
        private readonly ISaleTypeRepository _saleTypeRepository;

        public GetAllSaleTypesQueryHandler(ISaleTypeRepository saleTypeRepository)
        {
            _saleTypeRepository = saleTypeRepository;
        }

        /// <summary>
        /// Executes the query to get all sale types.
        /// </summary>
        /// <param name="query">The query request (no parameters required).</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>
        /// A list of <see cref="SaleTypeListDto"/> objects containing Id, Name, Description, and PropertyCount.
        /// Returns an empty list if no sale types are found (translated to 204 NoContent in the controller).
        /// </returns>
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
