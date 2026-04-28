using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IPropertyRepository : IGenericRepository<Property>
    {
        Task<List<Property>> GetPropertiesByAgentAsync(string agentId, bool onlyAvailable);
        Task<Property?> GetPropertyByIdAsync(int id);
        Task<List<Property>> GetAllProperties(bool onlyAvailable);
        Task<Property?> GetPropertyByCodeAsync(string code);
        Task<Property?> UpdatePropertyAsync(int id, Property entity);
        Task<bool> ExistsByCodeAsync(string code);
        Task<bool> DeletePropertyAsync(int id);
    }
}
