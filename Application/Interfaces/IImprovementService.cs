using Application.Dtos.Improvement;
using Application.Interfaces;

namespace Application.Services
{
    public interface IImprovementService : IGenericService<ImprovementDto>
    {
        Task<ImprovementDto?> AddAsync(ImprovementCreateDto dto);
        Task<ImprovementDto?> GetById(int id);
        Task<List<ImprovementListDto>> GetAllList();
        Task<ImprovementDto?> UpdateAsync(int id, ImprovementUpdateDto dto);
        Task DeleteAsync(int id);

    }
}
