using Business.Interface;
using Data.Interface;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Business.Business
{
    public class RocketBusiness : BusinessBase<Rocket, IRocketRepository>, IRocketBusiness, IBusiness
    {
        public RocketBusiness(IUnitOfWork uow):base(uow)
        {
             
        }
    }
}
