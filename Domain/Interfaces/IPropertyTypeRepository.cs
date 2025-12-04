using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IPropertyTypeRepository : IGenericRepository<PropertyType>
    {
        Task<PropertyType?> AddPropertyAsync(PropertyType entity);
        Task<PropertyType?> GetPropertyById(int id);
        Task<List<PropertyType>> GetAllPropertyList();
        Task<PropertyType?> UpdatePropertyAsync(int id, PropertyType entity);
        Task DeletePropertyAsync(int id);
    }
}
