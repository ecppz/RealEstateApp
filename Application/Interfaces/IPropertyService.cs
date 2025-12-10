using Application.Dtos.Property;
using Application.Interfaces;

namespace Application.Services
{
    public interface IPropertyService : IGenericService<PropertyDto>
    {
        Task<PropertyDto?> AddPropertyAsync(CreatePropertyDto dto);
        Task<List<PropertyDto>> GetProperties(string agentId, bool onlyAvailable); // obtiene por agentes
        Task<List<PropertyDto>> GetAllProperties(bool onlyAvailable);
        Task<PropertyDto?> GetPropertyById(int id);
        Task<PropertyDto?> UpdatePropertyAsync(int id, PropertyDto dto);
        Task<bool> DeletePropertyAsync(int id);
        Task<List<PropertyDto>> GetAvailablePropertiesAsync();

    }
}
