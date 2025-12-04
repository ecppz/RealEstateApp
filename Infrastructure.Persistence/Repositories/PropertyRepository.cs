using Domain.Common.Enums;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class PropertyRepository : GenericRepository<Property>, IPropertyRepository
    {
        public PropertyRepository(RealEstateAppContext context) : base(context) { }

        public async Task<List<Property>> GetPropertiesByAgentAsync(string agentId, bool onlyAvailable)
        {
            var query = context.Properties
                .Include(p => p.PropertyType)
                .Include(p => p.SaleType)
                .Include(p => p.Images)
                .Include(p => p.Improvements)
                    .ThenInclude(pi => pi.Improvement) 
                .AsQueryable();


            query = query.Where(p => p.AgentId == agentId);

            if (onlyAvailable)
                query = query.Where(p => p.Status == PropertyStatus.Available);

            return await query.ToListAsync();
        }

        public async Task<Property?> GetPropertyByIdAsync(int id)
        {
            return await context.Properties
                .Include(p => p.PropertyType)
                .Include(p => p.SaleType)
                .Include(p => p.Images)
                .Include(p => p.Improvements)
                    .ThenInclude(pi => pi.Improvement) 
                .FirstOrDefaultAsync(p => p.Id == id);
        }


        public async Task<bool> ExistsByCodeAsync(string code)
        {
            return await context.Properties.AnyAsync(p => p.Code == code);
        }

        public async Task<bool> DeletePropertyAsync(int id)
        {
            var property = await context.Properties
                .Include(p => p.Images)
                .Include(p => p.Improvements)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (property == null)
            {
                return false;
            }

            if (property.Images != null && property.Images.Any())
            {
                context.PropertyImages.RemoveRange(property.Images);
            }

            if (property.Improvements != null && property.Improvements.Any())
            {
                context.PropertyImprovements.RemoveRange(property.Improvements);
            }

            context.Properties.Remove(property);

            await context.SaveChangesAsync();
            return true;
        }

    }
}
