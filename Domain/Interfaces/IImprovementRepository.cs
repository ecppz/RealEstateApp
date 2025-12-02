using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IImprovementRepository : IGenericRepository<Improvement>
    {
        // Crear una mejora
        Task<Improvement?> AddAsync(Improvement entity);

        // Obtener una mejora por Id
        Task<Improvement?> GetById(int id);

        // Listar todas las mejoras
        Task<List<Improvement>> GetAllList();

        // Actualizar una mejora existente
        Task<Improvement?> UpdateAsync(int id, Improvement entity);

        // Eliminar una mejora
        Task DeleteAsync(int id);

    }
}
