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

        public async Task<PropertyTypeDto?> AddAsync(PropertyTypeCreateDto dto)
        {
            // Validar campos requeridos
            if (string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Description))
                throw new ArgumentException("El nombre y la descripción son requeridos.");

            // Validar duplicados
            var existingTypes = await propertyTypeRepository.GetAllList();
            if (existingTypes.Any(t => t.Name.ToLower() == dto.Name.ToLower()))
                throw new InvalidOperationException("Ya existe un tipo de propiedad con ese nombre.");

            // Mapear y guardar
            var entity = mapper.Map<PropertyType>(dto);
            var created = await propertyTypeRepository.AddAsync(entity);

            return mapper.Map<PropertyTypeDto>(created);
        }


        public async Task<PropertyTypeDto?> UpdateAsync(PropertyTypeUpdateDto dto, int id)
        {
            // Validar existencia
            var existing = await propertyTypeRepository.GetById(id);
            if (existing == null)
                throw new KeyNotFoundException("El tipo de propiedad no existe.");

            // Validar duplicados (excepto el mismo registro)
            var allTypes = await propertyTypeRepository.GetAllList();
            if (allTypes.Any(t => t.Name.ToLower() == dto.Name.ToLower() && t.Id != id))
                throw new InvalidOperationException("Ya existe otro tipo de propiedad con ese nombre.");

            // Mapear y actualizar
            var entity = mapper.Map<PropertyType>(dto);
            var updated = await propertyTypeRepository.UpdateAsync(id, entity);

            return mapper.Map<PropertyTypeDto>(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            // Validar existencia
            var existing = await propertyTypeRepository.GetById(id);
            if (existing == null)
                throw new KeyNotFoundException("El tipo de propiedad no existe.");

            // Validar si tiene propiedades asociadas
            if (existing.Properties != null && existing.Properties.Any())
                throw new InvalidOperationException("No se puede eliminar un tipo de propiedad con propiedades asociadas.");

            await propertyTypeRepository.DeleteAsync(id);
            return true;
        }


        public async Task<PropertyTypeDto?> GetById(int id)
        {
            var entity = await propertyTypeRepository.GetById(id);
            if (entity == null)
                throw new KeyNotFoundException("El tipo de propiedad no existe.");

            return mapper.Map<PropertyTypeDto>(entity);
        }


        public async Task<List<PropertyTypeListDto>> GetAll()
        {
            var entities = await propertyTypeRepository.GetAllList();

            // Mapear a listado con conteo
            var result = mapper.Map<List<PropertyTypeListDto>>(entities);
            return result;
        }


    }
}
