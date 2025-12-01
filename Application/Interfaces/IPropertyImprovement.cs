using Application.Dtos.PropertyImprovement;

namespace Application.Interfaces
{
    public interface IPropertyImprovement : IGenericService<PropertyImprovementDto>
    {
        Task<PropertyImprovementDto?> AddAsync(PropertyImprovementCreateDto dto);
        Task<PropertyImprovementDto?> GetById(int id);
        Task<List<PropertyImprovementListDto>> GetAllList();
        Task<List<PropertyImprovementDto>> GetByPropertyId(int propertyId);
        Task<PropertyImprovementDto?> UpdateAsync(int id, PropertyImprovementUpdateDto dto);
        Task DeleteAsync(int id);

    }
}
