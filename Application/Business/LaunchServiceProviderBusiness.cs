using Application.Interface;
using Domain.Entities;
using Domain.Interface;

namespace Application.Business
{
    public class LaunchServiceProviderBusiness : BusinessBase<LaunchServiceProvider, ILaunchServiceProviderRepository>, ILaunchServiceProviderBusiness, IBusiness
    {
        public LaunchServiceProviderBusiness(IUnitOfWork uow):base(uow)
        {
            
        }
    }
}
