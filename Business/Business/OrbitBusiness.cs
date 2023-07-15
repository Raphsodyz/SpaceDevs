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
    public class OrbitBusiness : BusinessBase<Orbit, IOrbitRepository>, IOrbitBusiness, IBusiness
    {
        public OrbitBusiness(IUnitOfWork uow):base(uow)
        {
            
        }
    }
}
