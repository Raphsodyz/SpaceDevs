using Domain.Entities;
using Domain.Interface;
using Infrastructure.Persistence.Context;

namespace Infrastructure.Persistence.Repository
{
    public class LaunchServiceProviderRepository : GenericRepository<LaunchServiceProvider>, ILaunchServiceProviderRepository
    {
        public LaunchServiceProviderRepository(FutureSpaceContext context):base(context)
        {
            
        }
    }
}
