using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IPropertyRepository : IGenericRepository<Property>
    {
        Task<List<Property>> GetPropertiesByAgentAsync(string agentId, bool onlyAvailable);
        Task<Property?> GetPropertyByIdAsync(int id);
        Task<bool> ExistsByCodeAsync(string code);
    }
}
