using Application.Dtos.Property;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class PropertyService : GenericService<Property, PropertyDto>, IPropertyService
    {
        private readonly IPropertyRepository propertyRepository;
        private readonly IMapper mapper;
        public PropertyService(IPropertyRepository propertyRepository, IMapper mapper) : base(propertyRepository, mapper)
        {
            this.propertyRepository = propertyRepository;
            this.mapper = mapper;
        }
    }
}
