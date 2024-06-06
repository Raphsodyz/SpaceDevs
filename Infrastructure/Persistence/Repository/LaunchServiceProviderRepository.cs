using Domain.Entities;
using Domain.Interface;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Context.Factory;

namespace Infrastructure.Persistence.Repository
{
    public class LaunchServiceProviderRepository : GenericRepository<LaunchServiceProvider>, ILaunchServiceProviderRepository
    {
        public LaunchServiceProviderRepository(IDbContextFactory contexts):base(contexts)
        {
            
        }
    }
}
