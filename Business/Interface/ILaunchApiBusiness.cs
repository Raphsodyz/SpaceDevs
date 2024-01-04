using Data.Interface;
using Domain.Entities;
using Cross.Cutting.Helper;
using Data.Materializated.Views;
using Business.DTO.Entities;

namespace Business.Interface
{
    public interface ILaunchApiBusiness : IBusinessBase<Launch, ILaunchRepository>
    {
        Task<LaunchView> GetOneLaunch(Guid? launchId);
        Task<Pagination<LaunchView>> GetAllLaunchPaged(int? page);
        Task SoftDeleteLaunch(Guid? launchId);
        Task<LaunchView> UpdateLaunch(Guid? launchId);
        Task<bool> UpdateDataSet(int? skip);
        Task<Pagination<LaunchView>> SearchByParam(SearchLaunchRequest request);
    }
}
