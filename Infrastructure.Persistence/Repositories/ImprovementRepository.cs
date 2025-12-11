using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class ImprovementRepository : GenericRepository<Improvement>, IImprovementRepository
    {
        public ImprovementRepository(RealEstateAppContext context) : base(context)
        {

        }

        public async Task<Improvement?> AddAsync(Improvement entity)
        {
            await context.Improvements.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<Improvement?> GetById(int id)
        {
            return await context.Improvements.FindAsync(id);
        }

        public async Task<List<Improvement>> GetAllList()
        {
            return await context.Improvements.ToListAsync();
        }

        public async Task<Improvement?> UpdateAsync(int id, Improvement entity)
        {
            var existing = await context.Improvements.FindAsync(id);
            if (existing != null)
            {
                existing.Name = entity.Name;
                existing.Description = entity.Description;

                await context.SaveChangesAsync();
                return existing;
            }
            return null;
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await context.Improvements.FindAsync(id);
            if (existing != null)
            {
                context.Improvements.Remove(existing);
                await context.SaveChangesAsync();
            }
        }

    }
}
