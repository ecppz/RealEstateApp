using Application.Dtos.PropertyType;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class PropertyTypeService : GenericService<PropertyType, PropertyTypeDto>, IPropertyTypeService
    {
        private readonly IPropertyTypeRepository propertyTypeRepository;
        private readonly IMapper mapper;

        public PropertyTypeService(IPropertyTypeRepository propertyTypeRepository, IMapper mapper)
            : base(propertyTypeRepository, mapper)
        {
            this.propertyTypeRepository = propertyTypeRepository;
            this.mapper = mapper;
        }
    }
}
