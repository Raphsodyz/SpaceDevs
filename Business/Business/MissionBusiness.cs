using Business.Interface;
using Data.Interface;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Business
{
    public class MissionBusiness : BusinessBase<Mission, IMissionRepository>, IMissionBusiness, IBusiness
    {
        public MissionBusiness(IUnitOfWork uow):base(uow)
        {
            
        }
    }
}
