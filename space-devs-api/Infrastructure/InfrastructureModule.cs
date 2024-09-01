using Core.Database.Repository;
using Core.ExternalServices;
using Flurl.Http;
using Flurl.Http.Configuration;
using Infrastructure.ExternalServices;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class InfrastructureModule
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString, string redisConnectionString)
        {
            return services
                .AddPostgresContext(connectionString)
                .AddRepositories(redisConnectionString)
                .AddAutoMapper()
                .AddFlurlBaseClient()
                .AddExternalServices();
        }

        private static IServiceCollection AddPostgresContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<BaseApiContext>(options => options.UseNpgsql(connectionString));
            return services;
        }

        private static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services, string redisConnectionString)
        {
            services.AddScoped<IConfigurationRepository, ConfigurationRepository>();
            services.AddScoped<ILaunchRepository, LaunchRepository>();
            services.AddScoped<ILaunchServiceProviderRepository, LaunchServiceProviderRepository>();
            services.AddScoped<ILaunchViewRepository, LaunchViewRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<IMissionRepository, MissionRepository>();
            services.AddScoped<IOrbitRepository, OrbitRepository>();
            services.AddScoped<IPadRepository, PadRepository>();
            services.AddScoped<IRocketRepository, RocketRepository>();
            services.AddScoped<IStatusRepository, StatusRepository>();
            services.AddScoped<IRedisRepository>(provider => new RedisRepository(redisConnectionString));

            return services;
        }

        public static IServiceCollection AddExternalServices(this IServiceCollection services)
        {
            services.AddScoped<ISpaceDevsUpdateService, SpaceDevsUpdateService>();
            return services;
        }

        private static IServiceCollection AddFlurlBaseClient(this IServiceCollection services)
        {
            services.AddSingleton<IFlurlClientCache>(_ => new FlurlClientCache()
                .WithDefaults(builder => builder
                    .WithSettings(cfg => {
                        cfg.Timeout = TimeSpan.FromSeconds(10);
                        cfg.AllowedHttpStatusRange = "*";
                        cfg.Redirects.Enabled = true;
                    })
                )
            );

            return services;
        }
    }
}