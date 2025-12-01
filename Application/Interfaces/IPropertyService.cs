using Application.Dtos.Property;
using Application.Interfaces;

namespace Application.Services
{
    public interface IPropertyService : IGenericService<PropertyDto> 
    {
        Task<PropertyDto?> AddAsync(PropertyDto dto);
        Task<PropertyDto?> UpdateAsync(PropertyDto dto, int id);
        Task<bool> DeleteAsync(int id);
        Task<PropertyDto?> GetById(int id);
        Task<List<PropertyDto>> GetAll();

    }
}
