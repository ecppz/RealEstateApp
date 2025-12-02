using Application.Dtos.PropertyImprovement;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class PropertyImprovementService : GenericService<PropertyImprovement, PropertyImprovementDto>, IPropertyImprovement
    {
        private readonly IPropertyImprovementRepository propertyImprovementRepository;
        private readonly IMapper mapper;

        public PropertyImprovementService(IPropertyImprovementRepository propertyImprovementRepository, IMapper mapper)
            : base(propertyImprovementRepository, mapper)
        {
            this.propertyImprovementRepository = propertyImprovementRepository;
            this.mapper = mapper;
        }

        // Crear relación propiedad-mejora
        public async Task<PropertyImprovementDto?> AddAsync(PropertyImprovementCreateDto dto)
        {
            var entity = mapper.Map<PropertyImprovement>(dto);
            var created = await propertyImprovementRepository.AddAsync(entity);
            return mapper.Map<PropertyImprovementDto?>(created);
        }

        // Obtener relación por Id
        public async Task<PropertyImprovementDto?> GetById(int id)
        {
            var entity = await propertyImprovementRepository.GetById(id);
            return mapper.Map<PropertyImprovementDto?>(entity);
        }

        // Listar todas las relaciones
        public async Task<List<PropertyImprovementListDto>> GetAllList()
        {
            var entities = await propertyImprovementRepository.GetAllList();
            return mapper.Map<List<PropertyImprovementListDto>>(entities);
        }

        // Listar todas las mejoras asociadas a una propiedad
        public async Task<List<PropertyImprovementDto>> GetByPropertyId(int propertyId)
        {
            var entities = await propertyImprovementRepository.GetByPropertyId(propertyId);
            return mapper.Map<List<PropertyImprovementDto>>(entities);
        }

        // Actualizar relación propiedad-mejora
        public async Task<PropertyImprovementDto?> UpdateAsync(int id, PropertyImprovementUpdateDto dto)
        {
            var entity = mapper.Map<PropertyImprovement>(dto);
            var updated = await propertyImprovementRepository.UpdateAsync(id, entity);
            return mapper.Map<PropertyImprovementDto?>(updated);
        }

        // Eliminar relación propiedad-mejora
        public async Task DeleteAsync(int id)
        {
            await propertyImprovementRepository.DeleteAsync(id);
        }


    }
}
