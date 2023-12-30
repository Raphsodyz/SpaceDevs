using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Materializated.Views;

namespace Data.Interface
{
    public interface ILaunchViewRepository : IGenericViewRepository<LaunchView>
    {
        Task<bool> ViewExists();
    }
}