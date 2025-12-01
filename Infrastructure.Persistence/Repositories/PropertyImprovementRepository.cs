using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class PropertyImprovementRepository : GenericRepository<PropertyImprovement>, IPropertyImprovementRepository
    {
        public PropertyImprovementRepository(RealEstateAppContext context) : base(context)
        {

        }

        public async Task<PropertyImprovement?> AddAsync(PropertyImprovement entity)
        {
            await context.PropertyImprovements.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<PropertyImprovement?> GetById(int id)
        {
            return await context.PropertyImprovements
                .Include(pi => pi.Improvement)
                .Include(pi => pi.Property)
                .FirstOrDefaultAsync(pi => pi.Id == id);
        }

        public async Task<List<PropertyImprovement>> GetAllList()
        {
            return await context.PropertyImprovements
                .Include(pi => pi.Improvement)
                .Include(pi => pi.Property)
                .ToListAsync();
        }

        public async Task<List<PropertyImprovement>> GetByPropertyId(int propertyId)
        {
            return await context.PropertyImprovements
                .Include(pi => pi.Improvement)
                .Where(pi => pi.PropertyId == propertyId)
                .ToListAsync();
        }

        public async Task<PropertyImprovement?> UpdateAsync(int id, PropertyImprovement entity)
        {
            var existing = await context.PropertyImprovements.FindAsync(id);
            if (existing != null)
            {
                context.Entry(existing).CurrentValues.SetValues(entity);
                await context.SaveChangesAsync();
                return existing;
            }
            return null;
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await context.PropertyImprovements.FindAsync(id);
            if (existing != null)
            {
                context.PropertyImprovements.Remove(existing);
                await context.SaveChangesAsync();
            }
        }


    }
}
