using Application.Dtos.Improvement;
using Application.Dtos.Property;
using Application.Dtos.PropertyImprovement;
using Application.Dtos.PropertyType;
using Domain.Interfaces;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.PropertyType.Queries.GetById
{
    /// <summary>
    /// Query to retrieve a property type by its unique identifier.
    /// </summary>
    public class GetPropertyTypeByIdQuery : IRequest<PropertyTypeListDto?>
    {
        /// <example>5</example>
        [SwaggerParameter(Description = "The unique identifier of the property type to retrieve")]
        public int Id { get; set; }
    }

    /// <summary>
    /// Handles the retrieval of a property type by Id.
    /// </summary>
    public class GetPropertyTypeByIdQueryHandler : IRequestHandler<GetPropertyTypeByIdQuery, PropertyTypeListDto?>
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository;

        public GetPropertyTypeByIdQueryHandler(IPropertyTypeRepository propertyTypeRepository)
        {
            _propertyTypeRepository = propertyTypeRepository;
        }

        /// <summary>
        /// Executes the query to get a property type by Id.
        /// </summary>
        /// <param name="query">The query containing the Id of the property type.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>
        /// A <see cref="PropertyTypeListDto"/> with Id, Name, Description, and PropertyCount if found;
        /// otherwise <c>null</c> (translated to 204 NoContent in the controller).
        /// </returns>
        public async Task<PropertyTypeListDto?> Handle(GetPropertyTypeByIdQuery query, CancellationToken cancellationToken)
        {
            var propertyType = await _propertyTypeRepository.GetPropertyById(query.Id);

            if (propertyType == null)
            {
                return null; // El controlador luego traduce esto a 204 NoContent
            }

            return new PropertyTypeListDto
            {
                Id = propertyType.Id,
                Name = propertyType.Name,
                Description = propertyType.Description,
                PropertyCount = propertyType.Properties?.Count ?? 0
            };
        }
    }
}
