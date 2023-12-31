using Business.DTO;
using Data.Interface;
using Domain.Entities;
using Cross.Cutting.Helper;
using Data.Materializated.Views;

namespace Business.Interface
{
    public interface ILaunchApiBusiness : IBusinessBase<Launch, ILaunchRepository>
    {
        Task<LaunchView> GetOneLaunch(Guid? launchId);
        Task<Pagination<LaunchView>> GetAllLaunchPaged(int? page);
        Task SoftDeleteLaunch(Guid? launchId);
        Task<LaunchDTO> UpdateLaunch(Guid? launchId);
        Task<bool> UpdateDataSet(int? skip);
        Task<Pagination<LaunchView>> SearchByParam(string mission, string rocket, string location, string pad, string launch, int? page = null);
    }
}
