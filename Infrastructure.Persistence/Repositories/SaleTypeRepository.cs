using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence.Contexts;

namespace Infrastructure.Persistence.Repositories
{
    public class SaleTypeRepository : GenericRepository<SaleType>, ISaleTypeRepository
    {
        public SaleTypeRepository(RealEstateAppContext context) : base(context) { }
    }
}
