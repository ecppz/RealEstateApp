using Application.Dtos.Agent;
using Application.Dtos.Property;
using Application.Dtos.PropertyImage;
using Application.Dtos.PropertyImprovement;
using Application.Dtos.PropertyType;
using Application.Dtos.SaleType;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Properties.Queries.GetAll
{
    public class GetAgentPropertiesQuery : IRequest<IList<PropertyResponseDto>>
    {
        public required string AgentId { get; set; }
        public bool OnlyAvailable { get; set; } = false;
    }
    public class GetAgentPropertiesQueryHandler : IRequestHandler<GetAgentPropertiesQuery, IList<PropertyResponseDto>>
    {
        private readonly IPropertyRepository propertyRepository;

        public GetAgentPropertiesQueryHandler(IPropertyRepository propertyRepository)
        {
            this.propertyRepository = propertyRepository;
        }

        public async Task<IList<PropertyResponseDto>> Handle(GetAgentPropertiesQuery query, CancellationToken cancellationToken)
        {
            var listEntities = await propertyRepository.GetPropertiesByAgentAsync(query.AgentId, query.OnlyAvailable);

            var listDtos = listEntities.Select(p => new PropertyResponseDto
            {
                Id = p.Id,
                Code = p.Code,
                AgentId = p.AgentId,
                SaleTypeId = p.SaleTypeId,
                PropertyTypeId = p.PropertyTypeId,
                Price = p.Price,
                Description = p.Description,
                SizeInMeters = p.SizeInMeters,
                Bedrooms = p.Bedrooms,
                Bathrooms = p.Bathrooms,
                Status = p.Status,

                PropertyType = p.PropertyType != null ? new PropertyTypeResponseDto
                {
                    Id = p.PropertyType.Id,
                    Name = p.PropertyType.Name,
                    Description = p.PropertyType.Description
                } : null,

                SaleType = p.SaleType != null ? new SaleTypeResponseDto
                {
                    Id = p.SaleType.Id,
                    Name = p.SaleType.Name,
                    Description = p.SaleType.Description
                } : null,

                Images = p.Images?.Select(img => new PropertyImageResponseDto
                {
                    Id = img.Id,
                    ImageUrl = img.ImageUrl,
                    PropertyId = img.PropertyId
                }).ToList(),

                Improvements = p.Improvements?.Select(imp => new PropertyImprovementResponseDto
                {
                    PropertyId = imp.PropertyId,
                    ImprovementId = imp.ImprovementId
                }).ToList() ?? new List<PropertyImprovementResponseDto>()
            }).ToList();

            return listDtos;
        }


    }
}