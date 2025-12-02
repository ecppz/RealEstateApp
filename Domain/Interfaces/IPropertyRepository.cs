using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IPropertyRepository : IGenericRepository<Property>
    {
        // de mantenimiento de propiedades

        // Listado simple

        Task<List<Property>> GetAllList();

        // Listado con includes (ej. SaleType, PropertyType, Images, Improvements)

        Task<List<Property>> GetAllListWithInclude(List<string> properties);

        
    }
}
