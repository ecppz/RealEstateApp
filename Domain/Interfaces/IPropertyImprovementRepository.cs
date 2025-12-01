using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IPropertyImprovementRepository : IGenericRepository<PropertyImprovement>
    {
        // Crear una relación entre propiedad y mejora
        Task<PropertyImprovement?> AddAsync(PropertyImprovement entity);

        // Obtener una relación por Id
        Task<PropertyImprovement?> GetById(int id);

        // Listar todas las relaciones propiedad-mejora
        Task<List<PropertyImprovement>> GetAllList();

        // Listar todas las mejoras asociadas a una propiedad específica
        Task<List<PropertyImprovement>> GetByPropertyId(int propertyId);

        // Actualizar una relación propiedad-mejora
        Task<PropertyImprovement?> UpdateAsync(int id, PropertyImprovement entity);

        // Eliminar una relación propiedad-mejora
        Task DeleteAsync(int id);

    }
}
