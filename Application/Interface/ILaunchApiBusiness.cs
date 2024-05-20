using Domain.Entities;
using Cross.Cutting.Helper;
using Business.Request;
using Domain.Materializated.Views;
using Domain.Interface;
using Application.Interface;

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
