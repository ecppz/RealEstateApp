using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class PropertyRepository : GenericRepository<Property>, IPropertyRepository
    {
        public PropertyRepository(RealEstateAppContext context) : base(context)
        {
        
        }

        public override async Task<List<Property>> GetAllList()
        {
            return await context.Properties.ToListAsync();
        }

        public override async Task<Property?> GetById(int id)
        {
            return await context.Properties.FindAsync(id);
        }

        public override async Task<Property?> AddAsync(Property entity)
        {
            await context.Properties.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public override async Task<Property?> UpdateAsync(int id, Property entity)
        {
            var entry = await context.Properties.FindAsync(id);
            if (entry != null)
            {
                context.Entry(entry).CurrentValues.SetValues(entity);
                await context.SaveChangesAsync();
                return entry;
            }
            return null;
        }

        public override async Task DeleteAsync(int id)
        {
            var entity = await context.Properties.FindAsync(id);
            if (entity != null)
            {
                context.Properties.Remove(entity);
                await context.SaveChangesAsync();
            }
        }






    }
}
