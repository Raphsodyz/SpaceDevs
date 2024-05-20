using Domain.Interface;
using Domain.Materializated.Views;

namespace Application.Interface
{
    public interface ILaunchViewBusiness : IBusinessViewBase<LaunchView, ILaunchViewRepository>
    {
        Task<bool> ViewExists();
        Task RefreshView();
    }
}