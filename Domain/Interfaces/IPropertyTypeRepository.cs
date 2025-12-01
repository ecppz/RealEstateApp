using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IPropertyTypeRepository : IGenericRepository<PropertyType>
    {

        Task<PropertyType?> AddAsync(PropertyType entity);
        Task<PropertyType?> GetById(int id);
        Task<List<PropertyType>> GetAllList();
        Task<PropertyType?> UpdateAsync(int id, PropertyType entity);
        Task DeleteAsync(int id);
    }
}
