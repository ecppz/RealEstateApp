using Application.Dtos.Property;
using Application.Dtos.PropertyImage;
using Application.Dtos.PropertyImprovement;
using Application.Dtos.PropertyType;
using Application.Dtos.SaleType;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace InvestmentApp.Core.Application.Features.Assets.Queries.GetAll
{
    public class GetAllPropertyQuery : IRequest<IList<PropertyDto>>
    {
        public bool OnlyAvailable { get; set; } = false;
    }
    public class GetAllPropertyQueryHandler : IRequestHandler<GetAllPropertyQuery, IList<PropertyDto>>
    {
        private readonly IPropertyRepository propertyRepository;
        private readonly IMapper mapper;

        public GetAllPropertyQueryHandler(IPropertyRepository propertyRepository, IMapper mapper)
        {
            this.propertyRepository = propertyRepository;
            this.mapper = mapper;
        }

        public async Task<IList<PropertyDto>> Handle(GetAllPropertyQuery query, CancellationToken cancellationToken)
        {
            var listEntities = await propertyRepository.GetAllProperties(query.OnlyAvailable);

            var listDtos = listEntities.Select(p => new PropertyDto
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
                PropertyType = p.PropertyType != null ? mapper.Map<PropertyTypeDto>(p.PropertyType) : null,
                SaleType = p.SaleType != null ? mapper.Map<SaleTypeDto>(p.SaleType) : null,
                Images = p.Images?.Select(img => mapper.Map<PropertyImageDto>(img)).ToList(),
                Improvements = p.Improvements?.Select(imp => mapper.Map<PropertyImprovementDto>(imp)).ToList() ?? new List<PropertyImprovementDto>()
            }).ToList();

            return listDtos;
        }
    }
}
