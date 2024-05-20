using Business.Interface;
using Domain.Entities;
using Domain.Interface;

namespace Application.Business
{
    public class UpdateLogBusiness : BusinessBase<UpdateLog, IUpdateLogRepository>, IUpdateLogBusiness, IBusiness
    {
        public UpdateLogBusiness(IUnitOfWork uow):base(uow)
        {
            
        }

        public async Task<int> LastOffSet()
        {
            return await _repository.LastOffSet();
        }
    }
}
