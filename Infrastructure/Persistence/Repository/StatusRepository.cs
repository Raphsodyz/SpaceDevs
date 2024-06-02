using Domain.Entities;
using Domain.Interface;
using Infrastructure.Persistence.Context.Factory;

namespace Infrastructure.Persistence.Repository
{
    public class StatusRepository : GenericRepository<Status>, IStatusRepository
    {
        public StatusRepository(DbContextFactory contexts):base(contexts)
        {
            
        }
    }
}
