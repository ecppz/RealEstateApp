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
    public class GetAllPropertiesQuery : IRequest<IList<PropertyResponseDto>>
    {
        public bool OnlyAvailable { get; set; } = false;
    }
    public class GetAllPropertiesQueryHandler : IRequestHandler<GetAllPropertiesQuery, IList<PropertyResponseDto>>
    {
        private readonly IPropertyRepository propertyRepository;
        private readonly IMapper mapper;

        public GetAllPropertiesQueryHandler(IPropertyRepository propertyRepository, IMapper mapper)
        {
            this.propertyRepository = propertyRepository;
            this.mapper = mapper;
        }

        public async Task<IList<PropertyResponseDto>> Handle(GetAllPropertiesQuery query, CancellationToken cancellationToken)
        {
            var listEntities = await propertyRepository.GetAllProperties(query.OnlyAvailable);

            var listDtos = listEntities.Select(p => new PropertyResponseDto
            {
                Id = p.Id,
                Code = p.Code,
                PropertyTypeId = p.PropertyTypeId,
                SaleTypeId = p.SaleTypeId,
                AgentId = p.AgentId,
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
                    PropertyId = img.PropertyId,  
                    ImageUrl = img.ImageUrl
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
