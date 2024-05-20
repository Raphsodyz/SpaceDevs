using Domain.Entities;
using Domain.Interface;

namespace Application.Interface
{
    public interface ILaunchServiceProviderBusiness : IBusinessBase<LaunchServiceProvider, ILaunchServiceProviderRepository>
    {
    }
}
