using Cross.Cutting.Helper;
using Domain.Entities;
using Domain.Materializated.Views;
using Domain.Request;

namespace Domain.Repository
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