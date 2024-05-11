using Data.Interface;
using Domain.Entities;
using Cross.Cutting.Helper;
using Data.Materializated.Views;
using Business.Request;

namespace Business.Interface
{
    public interface ILaunchApiBusiness : IBusinessBase<Launch, ILaunchRepository>
    {
        Task<LaunchView> GetOneLaunch(Guid? launchId);
        Task<Pagination<LaunchView>> GetAllLaunchPaged(int? page);
        Task SoftDeleteLaunch(Guid? launchId);
        Task<LaunchView> UpdateLaunch(Guid? launchId);
        Task<bool> UpdateDataSet(UpdateLaunchRequest request);
        Task<Pagination<LaunchView>> SearchByParam(SearchLaunchRequest request);
    }
}
