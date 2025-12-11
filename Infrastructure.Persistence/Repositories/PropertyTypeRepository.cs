using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class PropertyTypeRepository : GenericRepository<PropertyType>, IPropertyTypeRepository
    {
        public PropertyTypeRepository(RealEstateAppContext context) : base(context) { }

        public async Task<PropertyType?> AddPropertyAsync(PropertyType entity)
        {
            await context.PropertyTypes.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<List<PropertyType>> GetAllPropertyList()
        {
            return await context.PropertyTypes
                .Include(pt => pt.Properties)
                .ToListAsync();
        }

        public async Task<PropertyType?> GetPropertyById(int id)
        {
            return await context.PropertyTypes
                .Include(pt => pt.Properties)
                .FirstOrDefaultAsync(pt => pt.Id == id);
        }

        public async Task<PropertyType?> UpdatePropertyAsync(int id, PropertyType entity)
        {
            var entry = await context.PropertyTypes.FindAsync(id);
            if (entry != null)
            {
                entry.Name = entity.Name;
                entry.Description = entity.Description;

                await context.SaveChangesAsync();
                return entry;
            }
            return null;
        }

        public async Task DeletePropertyAsync(int id)
        {
            var entity = await context.PropertyTypes
                .Include(pt => pt.Properties)
                .FirstOrDefaultAsync(pt => pt.Id == id);

            if (entity != null)
            {
                context.PropertyTypes.Remove(entity);
                await context.SaveChangesAsync();
            }
        }

    }
}