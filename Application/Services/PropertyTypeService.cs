using Application.Dtos.PropertyType;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class PropertyTypeService : GenericService<PropertyType, PropertyTypeDto>, IPropertyTypeService
    {
        private readonly IPropertyTypeRepository propertyTypeRepository;
        private readonly IMapper mapper;
        private readonly IBaseAccountService accountService;


        public PropertyTypeService(IPropertyTypeRepository propertyTypeRepository, IMapper mapper, IBaseAccountService accountService)
            : base(propertyTypeRepository, mapper)
        {
            this.propertyTypeRepository = propertyTypeRepository;
            this.mapper = mapper;
            this.accountService = accountService;
        }

        public async Task<PropertyTypeDto?> AddPropertyAsync(PropertyTypeCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Description))
                return null; // campos inválidos

            var existingTypes = await propertyTypeRepository.GetAllPropertyList();
            if (existingTypes.Any(t => t.Name.ToLower() == dto.Name.ToLower()))
                return null; // duplicado

            var entity = mapper.Map<PropertyType>(dto);
            var created = await propertyTypeRepository.AddAsync(entity);
            return mapper.Map<PropertyTypeDto>(created);
        }

        public async Task<PropertyTypeDto?> UpdatePropertyAsync(PropertyTypeUpdateDto dto, int id)
        {
            var existing = await propertyTypeRepository.GetPropertyById(id);
            if (existing == null)
                return null; // no existe

            var allTypes = await propertyTypeRepository.GetAllList();
            if (allTypes.Any(t => t.Name.ToLower() == dto.Name.ToLower() && t.Id != id))
                return null; // duplicado

            var entity = mapper.Map<PropertyType>(dto);
            var updated = await propertyTypeRepository.UpdatePropertyAsync(id, entity);
            return mapper.Map<PropertyTypeDto>(updated);
        }

        public async Task<bool> DeletePropertyAsync(int id)
        {
            var existing = await propertyTypeRepository.GetPropertyById(id);
            if (existing == null) return false; // no existe

            if (existing.Properties != null && existing.Properties.Any())
                return false; // tiene propiedades asociadas

            await propertyTypeRepository.DeletePropertyAsync(id);
            return true;
        }

        public async Task<PropertyTypeDto?> GetPropertyById(int id)
        {
            var entity = await propertyTypeRepository.GetPropertyById(id);
            if (entity == null) return null; // no existe

            return mapper.Map<PropertyTypeDto>(entity);
        }

        public async Task<List<PropertyTypeListDto>> GetAllPropertyList()
        {
            var entities = await propertyTypeRepository.GetAllList();

            var result = mapper.Map<List<PropertyTypeListDto>>(entities);
            return result;
        }


    }
}
