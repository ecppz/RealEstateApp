using Application.Dtos.Property;
using Application.Interfaces;

namespace Application.Services
{
    public interface IPropertyService : IGenericService<PropertyDto> 
    {
        Task<PropertyDto?> AddPropertyAsync(CreatePropertyDto dto);
        Task<List<PropertyDto>> GetProperties(string agentId, bool onlyAvailable);
        Task<PropertyDto?> GetPropertyById(int id);
        Task<bool> DeletePropertyAsync(int id);

    }
}
