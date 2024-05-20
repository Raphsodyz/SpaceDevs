using Domain.Materializated.Views;

namespace Domain.Interface
{
    public interface ILaunchViewRepository : IGenericViewRepository<LaunchView>
    {
        Task<bool> ViewExists();
        Task RefreshView();
    }
}