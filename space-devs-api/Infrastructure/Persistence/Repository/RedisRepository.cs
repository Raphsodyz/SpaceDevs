using System.Text;
using Core.CQRS.Queries.Launch.Requests;
using Core.Database.Repository;
using Core.Materializated.Views;
using Cross.Cutting.Helper;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Infrastructure.Persistence.Repository
{
    public class RedisRepository : IRedisRepository
    {
        private readonly IDatabase _cache;

        public RedisRepository(string redisConnectionString)
        {
            var redis = ConnectionMultiplexer.Connect(redisConnectionString);
            _cache = redis.GetDatabase();
        }

        public async Task<LaunchView> GetLaunchById(Guid? launchId)
        {
            var cachedLaunch = await _cache.StringGetAsync(RedisCollectionsKeys.SingleLaunchKey + launchId);
            if (cachedLaunch.IsNullOrEmpty)
                return null;

            return JsonConvert.DeserializeObject<LaunchView>(cachedLaunch);
        }

        public async Task<Pagination<LaunchView>> GetPagination(int? page)
        {
            var cachedPaginatedLaunch = await _cache.StringGetAsync(RedisCollectionsKeys.PaginatatedLaunchKey + (page ?? 0));
            if (cachedPaginatedLaunch.IsNullOrEmpty)
                return null;

            return JsonConvert.DeserializeObject<Pagination<LaunchView>>(cachedPaginatedLaunch);
        }

        public async Task<Pagination<LaunchView>> GetFromSearch(SearchLaunchRequest searchParams)
        {
            string key = RedisCollectionsKeys.SearchLaunchKey + SetupSearchCacheKey(searchParams);
            string page = searchParams?.Page == null ? "0" : searchParams.Page.ToString();
            var serializedSearchPagination = await _cache.HashGetAsync(key, page);

            if (serializedSearchPagination.IsNullOrEmpty)
                return null;

            return JsonConvert.DeserializeObject<Pagination<LaunchView>>(serializedSearchPagination);
        }

        public async Task SetLaunch(LaunchView launchView, TimeSpan? ttl = null)
        {
            string key = RedisCollectionsKeys.SingleLaunchKey + launchView.Id;
            var serializedLaunch = JsonConvert.SerializeObject(launchView);
            await _cache.StringSetAsync(key, serializedLaunch, ttl ?? TimeSpan.FromMinutes(RedisDefaultMinutesTTL.LargeRedisTTL));
        }

        public async Task SetPagination(int? page, Pagination<LaunchView> pagination, TimeSpan? ttl = null)
        {
            string key = RedisCollectionsKeys.PaginatatedLaunchKey + (page ?? 0);
            string serializedPagination = JsonConvert.SerializeObject(pagination);
            await _cache.StringSetAsync(key, serializedPagination, ttl ?? TimeSpan.FromMinutes(RedisDefaultMinutesTTL.LargeRedisTTL));
        }

        public async Task SetSearchPagination(SearchLaunchRequest searchParams, Pagination<LaunchView> pagination, TimeSpan? ttl = null)
        {
            string key = RedisCollectionsKeys.SearchLaunchKey + SetupSearchCacheKey(searchParams);
            string page = searchParams?.Page == null ? "0" : searchParams.Page.ToString();
            string serializedSearchPagination = JsonConvert.SerializeObject(pagination);

            await _cache.HashSetAsync(key, page, serializedSearchPagination);
            await _cache.KeyExpireAsync(key, ttl ?? TimeSpan.FromMinutes(RedisDefaultMinutesTTL.LowRedisTTL));
        }

        private string SetupSearchCacheKey(SearchLaunchRequest searchParams)
        {
            if (searchParams == null)
                return null;

            StringBuilder builder = new();
            foreach (var property in typeof(SearchLaunchRequest).GetProperties())
            {
                if(property.Name == nameof(searchParams.Page))
                    continue;

                var value = property.GetValue(searchParams);
                if (value != null && !value.Equals(GetDefault(property.PropertyType)))
                    builder.Append(value + "_");
            }

            if(builder.Length == 0)
                return null;

            builder.Length--;
            return builder.ToString();
        }

        private static object GetDefault(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
    }
}