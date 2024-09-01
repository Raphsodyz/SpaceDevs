using Core.Database.Repository;
using Core.Domain.Entities;
using Infrastructure.Persistence.Context;

namespace Infrastructure.Persistence.Repository
{
    public class LaunchServiceProviderRepository(BaseApiContext context) : GenericRepository<LaunchServiceProvider>(context), ILaunchServiceProviderRepository
    {
        
    }
}