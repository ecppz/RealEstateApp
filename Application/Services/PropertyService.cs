using Application.Dtos.Property;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class PropertyService : GenericService<Property, PropertyDto>, IPropertyService
    {
        private readonly IPropertyRepository propertyRepository;
        private readonly IMapper mapper;
        private readonly IBaseAccountService accountService;
        public PropertyService(IPropertyRepository propertyRepository, IMapper mapper, IBaseAccountService accountService) : base(propertyRepository, mapper)
        {
            this.propertyRepository = propertyRepository;
            this.mapper = mapper;
            this.accountService = accountService;
        }

        public async Task<PropertyDto?> AddAsync(PropertyDto dto)
        {
            // Validar campos requeridos
            if (string.IsNullOrWhiteSpace(dto.Code) || dto.Price <= 0 || dto.SizeInMeters <= 0)
                throw new ArgumentException("Código, precio y tamaño en metros son requeridos y deben ser válidos.");

            // Validar duplicado de código
            var allProperties = await propertyRepository.GetAllList();
            if (allProperties.Any(p => p.Code == dto.Code))
                throw new InvalidOperationException("Ya existe una propiedad con ese código.");

            // Mapear y guardar
            var entity = mapper.Map<Property>(dto);
            var created = await propertyRepository.AddAsync(entity);

            return mapper.Map<PropertyDto>(created);
        }


        public async Task<PropertyDto?> UpdateAsync(PropertyDto dto, int id)
        {
            // Validar existencia
            var existing = await propertyRepository.GetById(id);
            if (existing == null)
                throw new KeyNotFoundException("La propiedad no existe.");

            // Validar duplicado de código (excepto el mismo registro)
            var allProperties = await propertyRepository.GetAllList();
            if (allProperties.Any(p => p.Code == dto.Code && p.Id != id))
                throw new InvalidOperationException("Ya existe otra propiedad con ese código.");

            // Mapear y actualizar
            var entity = mapper.Map<Property>(dto);
            var updated = await propertyRepository.UpdateAsync(id, entity);

            return mapper.Map<PropertyDto>(updated);
        }


        public async Task<bool> DeleteAsync(int id)
        {
            // Validar existencia
            var existing = await propertyRepository.GetById(id);
            if (existing == null)
                throw new KeyNotFoundException("La propiedad no existe.");

            await propertyRepository.DeleteAsync(id);
            return true;
        }


        public async Task<PropertyDto?> GetById(int id)
        {
            var entity = await propertyRepository.GetById(id);
            if (entity == null)
                throw new KeyNotFoundException("La propiedad no existe.");

            return mapper.Map<PropertyDto>(entity);
        }


        public async Task<List<PropertyDto>> GetAll()
        {
            var entities = await propertyRepository.GetAllList();
            return mapper.Map<List<PropertyDto>>(entities);
        }


    }
}
