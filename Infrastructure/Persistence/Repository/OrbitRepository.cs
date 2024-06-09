using Domain.Entities;
using Domain.Interface;
using Infrastructure.Persistence.Context;

namespace Infrastructure.Persistence.Repository
{
    public class OrbitRepository : GenericRepository<Orbit>, IOrbitRepository
    {
        public OrbitRepository(FutureSpaceContext context):base(context)
        {
            
        }
    }
}
