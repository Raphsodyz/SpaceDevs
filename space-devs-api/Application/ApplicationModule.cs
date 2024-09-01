using Microsoft.Extensions.DependencyInjection;
using Core.MediatR.Handlers;
using Application.Handlers.QueryHandlers.LaunchApi;
using Application.Handlers.CommandHandlers.LaunchApi;
using System.Reflection;

namespace Application
{
    public static class ApplicationModule
    {
        public static IServiceCollection AddApplicationModule(this IServiceCollection services)
        {
            return services
                .AddHandlers()
                .AddMediator();
        }

        private static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            services.AddScoped<IGetAllLaunchesPagedHandler, GetAllLaunchesPagedHandler>();
            services.AddScoped<IGetOneLaunchHandler, GetOneLaunchHandler>();
            services.AddScoped<ISearchByParamHandler, SearchByParamHandler>();
            services.AddScoped<ISoftDeleteLaunchHandler, SoftDeleteLaunchHandler>();
            services.AddScoped<IUpdateDataSetHandler, UpdateDataSetHandler>();
            services.AddScoped<IUpdateOneLaunchHandler, UpdateOneLaunchHandler>();

            return services;
        }

        private static IServiceCollection AddMediator(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.Lifetime = ServiceLifetime.Scoped;
            });

            return services;
        }
    }
}