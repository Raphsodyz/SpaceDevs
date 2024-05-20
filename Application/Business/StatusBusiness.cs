using Business.Interface;
using Domain.Entities;
using Domain.Interface;

namespace Application.Business
{
    public class StatusBusiness : BusinessBase<Status, IStatusRepository>, IStatusBusiness, IBusiness
    {
        public StatusBusiness(IUnitOfWork uow):base(uow)
        {
            
        }
    }
}
