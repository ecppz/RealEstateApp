using Application.Dtos.PropertyType;
using Application.Interfaces;

namespace Application.Services
{
    public interface IPropertyTypeService : IGenericService<PropertyTypeDto>
    {
        Task<PropertyTypeDto?> AddPropertyAsync(PropertyTypeCreateDto dto);
        Task<PropertyTypeDto?> GetPropertyById(int id);
        Task<List<PropertyTypeListDto>> GetAllPropertyList();
        Task<PropertyTypeDto?> UpdatePropertyAsync(PropertyTypeUpdateDto dto, int id);
        Task<bool> DeletePropertyAsync(int id);

    }
}
