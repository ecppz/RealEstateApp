using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class SaleTypeRepository : GenericRepository<SaleType>, ISaleTypeRepository
    {
        public SaleTypeRepository(RealEstateAppContext context) : base(context) { }


        public async Task<SaleType?> AddAsync(SaleType entity)
        {
            await context.SaleTypes.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<SaleType?> GetById(int id)
        {
            return await context.SaleTypes
                .Include(st => st.Properties)
                .FirstOrDefaultAsync(st => st.Id == id);
        }

        public async Task<List<SaleType>> GetAllList()
        {
            return await context.SaleTypes
                .Include(st => st.Properties)
                .ToListAsync();
        }

        public async Task<SaleType?> UpdateAsync(int id, SaleType entity)
        {
            var existing = await context.SaleTypes.FindAsync(id);
            if (existing == null)
                return null;

            existing.Name = entity.Name;
            existing.Description = entity.Description;

            context.SaleTypes.Update(existing);
            await context.SaveChangesAsync();
            return existing;
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await context.SaleTypes
                .Include(st => st.Properties)
                .FirstOrDefaultAsync(st => st.Id == id);

            if (existing != null)
            {
                context.SaleTypes.Remove(existing);
                await context.SaveChangesAsync();
            }
        }

    }
}
