using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ISaleTypeRepository : IGenericRepository<SaleType>
    {
        Task<SaleType?> AddAsync(SaleType entity);
        Task<SaleType?> GetById(int id);
        Task<List<SaleType>> GetAllList();
        Task<SaleType?> UpdateAsync(int id, SaleType entity);
        Task DeleteAsync(int id);

    }
}
