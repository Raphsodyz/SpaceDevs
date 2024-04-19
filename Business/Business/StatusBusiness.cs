using Business.Interface;
using Data.Interface;
using Domain.Entities;

namespace Business.Business
{
    public class StatusBusiness : BusinessBase<Status, IStatusRepository>, IStatusBusiness, IBusiness
    {
        public StatusBusiness(IUnitOfWork uow):base(uow)
        {
            
        }
    }
}
