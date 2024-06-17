using Domain.ExternalServices;
using Domain.Interface;
using Domain.Repository;
using Infrastructure.ExternalServices;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class InfrastructureModule
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services
                .AddContext()
                .AddRepositories()
                .AddExternalServices()
                .AddAutoMapper();

            return services;
        }

        public static IServiceCollection AddContext(this IServiceCollection services)
        {
            services.AddDbContext<FutureSpaceContext>();
            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IConfigurationRepository, ConfigurationRepository>();
            services.AddScoped<IGenericDapperRepository, GenericDapperRepository>();
            services.AddScoped<ILaunchRepository, LaunchRepository>();
            services.AddScoped<ILaunchServiceProviderRepository, LaunchServiceProviderRepository>();
            services.AddScoped<ILaunchViewRepository, LaunchViewRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<IMissionRepository, MissionRepository>();
            services.AddScoped<IOrbitRepository, OrbitRepository>();
            services.AddScoped<IPadRepository, PadRepository>();
            services.AddScoped<IRocketRepository, RocketRepository>();
            services.AddScoped<IStatusRepository, StatusRepository>();
            services.AddScoped<IUpdateLogRepository, UpdateLogRepository>();
            services.AddScoped<IRedisRepository, RedisRepository>();
            
            return services;
        }

        public static IServiceCollection AddExternalServices(this IServiceCollection services)
        {
            services.AddTransient<IRequestLaunchService, GetLaunchesFromSpaceDevs>();
            return services;
        }

        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            return services;
        }
    }
}