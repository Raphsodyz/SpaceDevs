using Flurl.Http;
using Flurl.Http.Configuration;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class InfrastructureModule
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
        {
            return services
                .AddPostgresContext(connectionString)
                .AddRepositorios()
                .AddAutoMapper()
                .AddFlurlBaseClient();
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

        private static IServiceCollection AddRepositorios(this IServiceCollection services)
        {
            
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