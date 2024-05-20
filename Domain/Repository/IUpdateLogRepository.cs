using Domain.Entities;

namespace Domain.Interface
{
    public interface IUpdateLogRepository : IGenericRepository<UpdateLog>
    {
        Task<int> LastOffSet();
    }
}
