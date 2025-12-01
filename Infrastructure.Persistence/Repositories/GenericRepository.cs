using Domain.Interfaces;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class GenericRepository<Entity> : IGenericRepository<Entity>
        where Entity : class        
    {
        protected readonly RealEstateAppContext context;

        public GenericRepository(RealEstateAppContext context)
        {
            this.context = context;
        }
        public virtual async Task<Entity?> AddAsync(Entity entity)
        {
            await context.Set<Entity>().AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }
        public virtual async Task<List<Entity>?> AddRangeAsync(List<Entity> entities)
        {
            await context.Set<Entity>().AddRangeAsync(entities);
            await context.SaveChangesAsync();
            return entities;
        }
        public virtual async Task<Entity?> UpdateAsync(int id, Entity entity)
        {
            var entry = await context.Set<Entity>().FindAsync(id);

            if (entry != null)
            {
                context.Entry(entry).CurrentValues.SetValues(entity);
                await context.SaveChangesAsync();
                return entry;
            }

            return null;
        }

        public virtual async Task<int> UpdateRangeAsync(IEnumerable<Entity> entities)
        {
            context.Set<Entity>().UpdateRange(entities);
            return await context.SaveChangesAsync();
        }
        public virtual async Task DeleteAsync(Guid id)
        {
            var entity = await context.Set<Entity>().FindAsync(id);
            if (entity != null)
            {
                context.Set<Entity>().Remove(entity);
                await context.SaveChangesAsync();
            }
        }

        public virtual async Task DeleteAsync(int id)
        {
            var entity = await context.Set<Entity>().FindAsync(id);
            if (entity != null)
            {
                context.Set<Entity>().Remove(entity);
                await context.SaveChangesAsync();
            }
        }
        public virtual async Task<List<Entity>> GetAllList()
        {
            return await context.Set<Entity>().ToListAsync(); //EF - immediate execution
        }

        public virtual async Task<List<Entity>> GetAllListWithInclude(List<string> properties)
        {
            var query = context.Set<Entity>().AsQueryable();

            foreach(var property in properties)
            {
                query = query.Include(property);
            }

            return await query.ToListAsync(); //EF - immediate execution
        }    
        public virtual async Task<Entity?> GetById(int id)
        {
            return await context.Set<Entity>().FindAsync(id);
        }
        public virtual IQueryable<Entity> GetAllQuery()
        {
            return context.Set<Entity>().AsQueryable();
        }
        public virtual IQueryable<Entity> GetAllQueryWithInclude(List<string> properties)
        {
            var query = context.Set<Entity>().AsQueryable();

            foreach (var property in properties)
            {
                query = query.Include(property);
            }

            return query; //EF - deffered execution
        }

    }
}