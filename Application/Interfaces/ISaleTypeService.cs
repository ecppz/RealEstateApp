using Application.Dtos.SaleType;
using Application.Interfaces;

namespace Application.Services
{
    public interface ISaleTypeService : IGenericService<SaleTypeDto>
    {
        // Crear un nuevo tipo de venta
        Task<SaleTypeDto?> AddAsync(SaleTypeCreateDto dto);

        // Obtener un tipo de venta por su Id
        Task<SaleTypeDto?> GetById(int id);

        // Listar todos los tipos de venta con conteo de propiedades asociadas
        Task<List<SaleTypeListDto>> GetAllList();

        // Actualizar un tipo de venta existente
        Task<SaleTypeDto?> UpdateAsync(SaleTypeUpdateDto dto, int id);

        // Eliminar un tipo de venta y sus propiedades asociadas
       // Task DeleteAsync(int id);

    }
}
