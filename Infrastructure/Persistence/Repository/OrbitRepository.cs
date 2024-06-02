using Domain.Entities;
using Domain.Interface;
using Infrastructure.Persistence.Context.Factory;

namespace Infrastructure.Persistence.Repository
{
    public class OrbitRepository : GenericRepository<Orbit>, IOrbitRepository
    {
        public OrbitRepository(DbContextFactory contexts):base(contexts)
        {
            
        }
    }
}
