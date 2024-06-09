using Domain.Entities;
using Domain.Interface;
using Infrastructure.Persistence.Context;

namespace Infrastructure.Persistence.Repository
{
    public class StatusRepository : GenericRepository<Status>, IStatusRepository
    {
        public StatusRepository(FutureSpaceContext context):base(context)
        {
            
        }
    }
}
