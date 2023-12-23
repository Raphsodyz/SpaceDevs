using Application.DTO;
using Data.Interface;
using Domain.Entities;
using Cross.Cutting.Helper;

namespace Business.Interface
{
    public interface ILaunchApiBusiness : IBusinessBase<Launch, ILaunchRepository>
    {
        LaunchDTO GetOneLaunch(Guid? launchId);
        Pagination<LaunchDTO> GetAllLaunchPaged(int? page);
        void SoftDeleteLaunch(Guid? launchId);
        Task<LaunchDTO> UpdateLaunch(Guid? launchId);
        Task<bool> UpdateDataSet(int? skip);
    }
}
