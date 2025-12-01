using Application.Dtos.PropertyType;
using Application.Interfaces;

namespace Application.Services
{
    public interface IPropertyTypeService : IGenericService<PropertyTypeDto>
    {
        Task<PropertyTypeDto?> AddAsync(PropertyTypeCreateDto dto);
        Task<PropertyTypeDto?> UpdateAsync(PropertyTypeUpdateDto dto, int id);
        Task<bool> DeleteAsync(int id);
        Task<PropertyTypeDto?> GetById(int id);
        Task<List<PropertyTypeListDto>> GetAll();

    }
}
