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
    public class GetPropertyTypeByIdQuery : IRequest<PropertyTypeDto>
    {
        public int Id { get; set; }
    }

    public class GetPropertyTypeByIdQueryHandler : IRequestHandler<GetPropertyTypeByIdQuery, PropertyTypeDto>
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository;

        public GetPropertyTypeByIdQueryHandler(IPropertyTypeRepository propertyTypeRepository)
        {
            _propertyTypeRepository = propertyTypeRepository;
        }

        public async Task<PropertyTypeDto> Handle(GetPropertyTypeByIdQuery query, CancellationToken cancellationToken)
        {
            var propertyType = await _propertyTypeRepository.GetPropertyById(query.Id);

            if (propertyType == null)
            {
                return null; // El controller luego traduce esto a 204 NoContent
            }

            var dto = new PropertyTypeDto
            {
                Id = propertyType.Id,
                Name = propertyType.Name,
                Description = propertyType.Description,
                Properties = propertyType.Properties?.Select(p => new PropertyDto
                {
                    Id = p.Id,
                    Code = p.Code,
                    PropertyTypeId = p.PropertyTypeId,
                    AgentId = p.AgentId,
                    SaleTypeId = p.SaleTypeId,
                    Price = p.Price,
                    Description = p.Description,
                    SizeInMeters = p.SizeInMeters,
                    Bedrooms = p.Bedrooms,
                    Bathrooms = p.Bathrooms,
                    Status = p.Status,
                    CreatedAt = p.CreatedAt,
                    Improvements = p.Improvements?.Select(i => new PropertyImprovementDto
                    {
                        PropertyId = i.PropertyId,
                        ImprovementId = i.ImprovementId,
                        Improvement = new ImprovementDto
                        {
                            Id = i.Improvement.Id,
                            Name = i.Improvement.Name,
                            Description = i.Improvement.Description
                        }
                    }).ToList() ?? new List<PropertyImprovementDto>()
                }).ToList()
            };

            return dto;
        }
    }
}
