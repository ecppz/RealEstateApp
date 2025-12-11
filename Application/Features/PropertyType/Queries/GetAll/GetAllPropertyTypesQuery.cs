using Application.Dtos.PropertyType;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.PropertyType.Queries.GetAll
{
    /// <summary>
    /// Query to retrieve all property types with their details and property count.
    /// </summary>
    public class GetAllPropertyTypesQuery : IRequest<IList<PropertyTypeListDto>>
    {
        // No parameters required for this query
    }

    /// <summary>
    /// Handles the retrieval of all property types.
    /// </summary>
    public class GetAllPropertyTypesQueryHandler : IRequestHandler<GetAllPropertyTypesQuery, IList<PropertyTypeListDto>>
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository;

        public GetAllPropertyTypesQueryHandler(IPropertyTypeRepository propertyTypeRepository)
        {
            _propertyTypeRepository = propertyTypeRepository;
        }

        /// <summary>
        /// Executes the query to get all property types.
        /// </summary>
        /// <param name="query">The query request (no parameters required).</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>
        /// A list of <see cref="PropertyTypeListDto"/> objects containing Id, Name, Description, and PropertyCount.
        /// Returns an empty list if no property types are found (translated to 204 NoContent in the controller).
        /// </returns>
        public async Task<IList<PropertyTypeListDto>> Handle(GetAllPropertyTypesQuery query, CancellationToken cancellationToken)
        {
            var propertyTypes = await _propertyTypeRepository.GetAllPropertyList();

            if (propertyTypes == null || !propertyTypes.Any())
            {
                return new List<PropertyTypeListDto>(); // Esto luego se traduce en 204 NoContent desde el controller
            }

            var listDtos = propertyTypes.Select(pt => new PropertyTypeListDto
            {
                Id = pt.Id,
                Name = pt.Name,
                Description = pt.Description,
                PropertyCount = pt.Properties?.Count ?? 0
            }).ToList();

            return listDtos;
        }
    }

}
