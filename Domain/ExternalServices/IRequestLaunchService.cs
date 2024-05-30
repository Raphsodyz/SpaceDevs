using Domain.Entities;

namespace Domain.ExternalServices
{
    public interface IRequestLaunchService
    {
        Task<List<Launch>> RequestLaunchSet(int limit, int offset, int entityCounter);
        Task<Launch> RequestLaunchById(Guid id);
    }
}