using Application.Business;
using Business.Interface;
using Domain.Entities;
using Domain.Interface;

namespace Application.Business
{
    public class OrbitBusiness : BusinessBase<Orbit, IOrbitRepository>, IOrbitBusiness, IBusiness
    {
        public OrbitBusiness(IUnitOfWork uow):base(uow)
        {
            
        }
    }
}
