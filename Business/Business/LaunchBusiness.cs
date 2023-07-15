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
    public class LaunchBusiness : BusinessBase<Launch, ILaunchRepository>, ILaunchBusiness, IBusiness
    {
        public LaunchBusiness(IUnitOfWork uow):base(uow)
        {
            
        }
    }
}
