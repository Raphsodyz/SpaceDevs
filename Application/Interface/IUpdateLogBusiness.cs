using Application.Interface;
using Domain.Entities;
using Domain.Interface;

namespace Business.Interface
{
    public interface IUpdateLogBusiness : IBusinessBase<UpdateLog, IUpdateLogRepository>
    {
        Task<int> LastOffSet();
    }
}
