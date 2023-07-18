using Application.DTO;
using Data.Interface;
using Domain.Entities;
using Domain.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interface
{
    public interface ILaunchApiBusiness : IBusinessBase<Launch, ILaunchRepository>
    {
        LaunchDTO GetOneLaunch(int? launchId);
        Pagination<LaunchDTO> GetAllLaunchPaged(int? page);
        void SoftDeleteLaunch(int? launchId);
        Task<LaunchDTO> UpdateLaunch(int? launchId);
        Task<bool> UpdateDataSet(int? skip);
    }
}
