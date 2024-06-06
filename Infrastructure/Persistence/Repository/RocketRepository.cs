using Domain.Entities;
using Domain.Interface;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Context.Factory;

namespace Infrastructure.Persistence.Repository
{
    public class RocketRepository : GenericRepository<Rocket>, IRocketRepository
    {
        public RocketRepository(IDbContextFactory contexts):base(contexts)
        {
            
        }
    }
}
