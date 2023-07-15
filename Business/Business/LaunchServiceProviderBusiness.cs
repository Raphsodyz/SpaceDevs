using Business.Interface;
using Data.Interface;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Business
{
    public class LaunchServiceProviderBusiness : BusinessBase<LaunchServiceProvider, ILaunchServiceProviderRepository>, ILaunchServiceProviderBusiness, IBusiness
    {
        public LaunchServiceProviderBusiness(IUnitOfWork uow):base(uow)
        {
            
        }
    }
}
