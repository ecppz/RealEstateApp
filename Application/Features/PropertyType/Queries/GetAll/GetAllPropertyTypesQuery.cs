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
    public class GetAllPropertyTypesQuery : IRequest<IList<PropertyTypeListDto>>
    {
    }

    public class GetAllPropertyTypesQueryHandler : IRequestHandler<GetAllPropertyTypesQuery, IList<PropertyTypeListDto>>
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository;

        public GetAllPropertyTypesQueryHandler(IPropertyTypeRepository propertyTypeRepository)
        {
            _propertyTypeRepository = propertyTypeRepository;
        }

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
