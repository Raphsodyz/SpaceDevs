using Core.CQRS.Queries.Launch.Requests;
using Core.Materializated.Views;
using Cross.Cutting.Helper;

namespace Core.Database.Repository
{
    public interface IRedisRepository
    {
        Task<LaunchView> GetLaunchById(Guid? launchId);
        Task<Pagination<LaunchView>> GetPagination(int? page);
        Task<Pagination<LaunchView>> GetFromSearch(SearchLaunchRequest searchParams);
        Task SetLaunch(LaunchView launch, TimeSpan? ttl = null);
        Task SetPagination(int? page, Pagination<LaunchView> pagination, TimeSpan? ttl = null);
        Task SetSearchPagination(SearchLaunchRequest searchParams, Pagination<LaunchView> pagination, TimeSpan? ttl = null);
    }
}