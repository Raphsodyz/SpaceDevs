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
    public class UpdateLogBusiness : BusinessBase<UpdateLog, IUpdateLogRepository>, IUpdateLogBusiness, IBusiness
    {
        public UpdateLogBusiness(IUnitOfWork uow):base(uow)
        {
            
        }
    }
}
