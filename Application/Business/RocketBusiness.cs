using Application.Business;
using Business.Interface;
using Domain.Entities;
using Domain.Interface;

namespace Application.Business
{
    public class RocketBusiness : BusinessBase<Rocket, IRocketRepository>, IRocketBusiness, IBusiness
    {
        public RocketBusiness(IUnitOfWork uow):base(uow)
        {
             
        }
    }
}
