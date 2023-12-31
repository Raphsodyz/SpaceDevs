using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Interface;
using Data.Materializated.Views;

namespace Business.Interface
{
    public interface ILaunchViewBusiness : IBusinessViewBase<LaunchView, ILaunchViewRepository>
    {
        Task<bool> ViewExists();
        Task RefreshView();
    }
}