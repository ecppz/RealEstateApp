using Application.Dtos.Improvement;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class ImprovementService : GenericService<Improvement, ImprovementDto>, IImprovementService
    {
        private readonly IImprovementRepository improvementRepository;
        private readonly IMapper mapper;

        public ImprovementService(IImprovementRepository improvementRepository, IMapper mapper)
            : base(improvementRepository, mapper)
        {
            this.improvementRepository = improvementRepository;
            this.mapper = mapper;
        }

        // Crear una mejora
        public async Task<ImprovementDto?> AddAsync(ImprovementCreateDto dto)
        {
            var entity = mapper.Map<Improvement>(dto);
            var created = await improvementRepository.AddAsync(entity);
            return mapper.Map<ImprovementDto>(created);
        }

        // Obtener una mejora por Id
        public async Task<ImprovementDto?> GetById(int id)
        {
            var entity = await improvementRepository.GetById(id);
            return mapper.Map<ImprovementDto?>(entity);
        }

        // Listar todas las mejoras
        public async Task<List<ImprovementDto>> GetAllList()
        {
            var entities = await improvementRepository.GetAllList();
            return mapper.Map<List<ImprovementDto>>(entities);
        }

        // Actualizar una mejora existente
        public async Task<ImprovementDto?> UpdateAsync(int id, ImprovementUpdateDto dto)
        {
            var entity = mapper.Map<Improvement>(dto);
            var updated = await improvementRepository.UpdateAsync(id, entity);
            return mapper.Map<ImprovementDto?>(updated);
        }

        // Eliminar una mejora
        public async Task DeleteAsync(int id)
        {
            await improvementRepository.DeleteAsync(id);
        }

    }
}
