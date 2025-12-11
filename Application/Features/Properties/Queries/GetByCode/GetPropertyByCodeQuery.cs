using Application.Dtos.Property;
using Application.Dtos.PropertyImage;
using Application.Dtos.PropertyImprovement;
using Application.Dtos.PropertyType;
using Application.Dtos.SaleType;
using Application.Exceptions;
using AutoMapper;
using Domain.Interfaces;
using MediatR;
using System.Net;

namespace Application.Features.Properties.Queries.GetByCode
{
    public class GetPropertyByCodeQuery : IRequest<PropertyResponseDto>
    {
        public required string Code { get; set; }
    }
    public class GetPropertyByCodeQueryHandler : IRequestHandler<GetPropertyByCodeQuery, PropertyResponseDto>
    {
        private readonly IPropertyRepository propertyRepository;
        private readonly IMapper mapper;

        public GetPropertyByCodeQueryHandler(IPropertyRepository propertyRepository, IMapper mapper)
        {
            this.propertyRepository = propertyRepository;
            this.mapper = mapper;
        }

        public async Task<PropertyResponseDto> Handle(GetPropertyByCodeQuery query, CancellationToken cancellationToken)
        {
            var entity = await propertyRepository.GetPropertyByCodeAsync(query.Code);

            if (entity == null) throw new ApiException("Property not found with this code", (int)HttpStatusCode.NotFound);

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
