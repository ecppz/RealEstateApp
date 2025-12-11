
using Application.Dtos.SaleType;
using Domain.Interfaces;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;


namespace Application.Features.SaleType.Queries.GetById
{
    /// <summary>
    /// Query to retrieve a sale type by its unique identifier.
    /// </summary>
    public class GetSaleTypeByIdQuery : IRequest<SaleTypeListDto?>
    {
        /// <example>3</example>
        [SwaggerParameter(Description = "The unique identifier of the sale type to retrieve")]
        public int Id { get; set; }
    }

    /// <summary>
    /// Handles the retrieval of a sale type by Id.
    /// </summary>
    public class GetSaleTypeByIdQueryHandler : IRequestHandler<GetSaleTypeByIdQuery, SaleTypeListDto?>
    {
        private readonly ISaleTypeRepository _saleTypeRepository;

        public GetSaleTypeByIdQueryHandler(ISaleTypeRepository saleTypeRepository)
        {
            _saleTypeRepository = saleTypeRepository;
        }

        /// <summary>
        /// Executes the query to get a sale type by Id.
        /// </summary>
        /// <param name="query">The query containing the Id of the sale type.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>
        /// A <see cref="SaleTypeListDto"/> with Id, Name, Description, and PropertyCount if found;
        /// otherwise <c>null</c> (translated to 204 NoContent in the controller).
        /// </returns>
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
