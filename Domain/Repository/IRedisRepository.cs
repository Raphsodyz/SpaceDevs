using Cross.Cutting.Helper;
using Domain.Entities;
using Domain.Request;

namespace Domain.Repository
{
    public interface IRedisRepository
    {
        Task<Launch> GetLaunchByIdAsync(Guid? launchId);
        Task<Pagination<Launch>> GetPaginationAsync(int? page);
        Task<Pagination<Launch>> GetPaginationAsync(SearchLaunchRequest searchParams);
        Task SetLaunchAsync(Launch launch, TimeSpan? ttl = null);
        Task SetPaginationAsync(int? page, Pagination<Launch> pagination, TimeSpan? ttl = null);
        Task SetSearchPaginationAsync(SearchLaunchRequest searchParams, Pagination<Launch> pagination, TimeSpan? ttl = null);
    }
}