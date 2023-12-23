using Business.Interface;
using Data.Interface;
using Domain.Entities;
using Cross.Cutting.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
