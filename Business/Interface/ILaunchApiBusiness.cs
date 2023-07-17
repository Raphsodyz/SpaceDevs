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
        Launch GetOneLaunch(int? launchId);
        Pagination<Launch> GetAllLaunchPaged(int? page);
        void HardDeleteLaunch(int? launchId);
        void SoftDeleteLaunch(int? launchId);
        Task<Launch> UpdateLaunch(int? launchId);
        Task<bool> UpdateDataSet(int? skip);
    }
}
