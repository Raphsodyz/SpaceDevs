using Core.Materializated.Views;

namespace Core.Database.Repository
{
    public interface ILaunchViewRepository : IGenericViewRepository<LaunchView>
    {
        Task<bool> ViewExists();
        Task RefreshView();
    }
}