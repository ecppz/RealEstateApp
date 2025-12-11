using Application.Dtos.Improvement;
using Application.Dtos.Property;
using Application.Dtos.PropertyImprovement;
using Application.Dtos.PropertyType;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.PropertyType.Queries.GetById
{
    public class GetPropertyTypeByIdQuery : IRequest<PropertyTypeListDto?>
    {
        public int Id { get; set; }
    }

    public class GetPropertyTypeByIdQueryHandler : IRequestHandler<GetPropertyTypeByIdQuery, PropertyTypeListDto?>
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository;

        public GetPropertyTypeByIdQueryHandler(IPropertyTypeRepository propertyTypeRepository)
        {
            _propertyTypeRepository = propertyTypeRepository;
        }

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
