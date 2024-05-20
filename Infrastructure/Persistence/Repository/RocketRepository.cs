using Domain.Entities;
using Domain.Interface;
using Infrastructure.Persistence.Context;

namespace Infrastructure.Persistence.Repository
{
    public class RocketRepository : GenericRepository<Rocket>, IRocketRepository
    {
        public RocketRepository(FutureSpaceContext context):base(context)
        {
            
        }
    }
}
