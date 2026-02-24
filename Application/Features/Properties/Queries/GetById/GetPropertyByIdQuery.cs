using Application.Dtos.Property;
using Application.Dtos.PropertyImage;
using Application.Dtos.PropertyImprovement;
using Application.Dtos.PropertyType;
using Application.Dtos.SaleType;
using Application.Exceptions;
using AutoMapper;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Application.Features.Properties.Queries.GetByCode
{
    public class GetPropertyByIdQuery : IRequest<PropertyResponseDto>
    {
        public required int Id { get; set; }
    }
    public class GetPropertyByIdQueryHandler : IRequestHandler<GetPropertyByIdQuery, PropertyResponseDto>
    {
        private readonly IPropertyRepository propertyRepository;
        private readonly IMapper mapper;

        public GetPropertyByIdQueryHandler(IPropertyRepository propertyRepository, IMapper mapper)
        {
            this.propertyRepository = propertyRepository;
            this.mapper = mapper;
        }

        public async Task<PropertyResponseDto> Handle(GetPropertyByIdQuery query, CancellationToken cancellationToken)
        {
            var entity = await propertyRepository.GetPropertyByIdAsync(query.Id);

            if (entity == null)
                throw new ApiException("Property not found with this id", (int)HttpStatusCode.NotFound);

            var dto = new PropertyResponseDto
            {
                Id = entity.Id,
                Code = entity.Code,
                AgentId = entity.AgentId,
                SaleTypeId = entity.SaleTypeId,
                PropertyTypeId = entity.PropertyTypeId,
                Price = entity.Price,
                Description = entity.Description,
                SizeInMeters = entity.SizeInMeters,
                Bedrooms = entity.Bedrooms,
                Bathrooms = entity.Bathrooms,
                Status = entity.Status,

                PropertyType = entity.PropertyType != null ? new PropertyTypeResponseDto
                {
                    Id = entity.PropertyType.Id,
                    Name = entity.PropertyType.Name,
                    Description = entity.PropertyType.Description
                } : null,

                SaleType = entity.SaleType != null ? new SaleTypeResponseDto
                {
                    Id = entity.SaleType.Id,
                    Name = entity.SaleType.Name,
                    Description = entity.SaleType.Description
                } : null,

                Images = entity.Images?.Select(img => new PropertyImageResponseDto
                {
                    Id = img.Id,
                    PropertyId = img.PropertyId,
                    ImageUrl = img.ImageUrl
                }).ToList(),

                Improvements = entity.Improvements?.Select(imp => new PropertyImprovementResponseDto
                {
                    PropertyId = imp.PropertyId,
                    ImprovementId = imp.ImprovementId
                }).ToList() ?? new List<PropertyImprovementResponseDto>()
            };

            return dto;
        }

    }
}
