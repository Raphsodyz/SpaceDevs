using Application.Handlers.CommandHandlers.LaunchApi;
using Application.Handlers.QueryHandlers.LaunchApi;
using Domain.Handlers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class ApplicationModule
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services
                .AddHandlers()
                .AddMediatR();

            return services;
        }

        public static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            services.AddScoped<IGetAllLaunchesPagedHandler, GetAllLaunchesPagedHandler>();
            services.AddScoped<IGetOneLaunchHandler, GetOneLaunchHandler>();
            services.AddScoped<ISearchByParamHandler, SearchByParamHandler>();
            services.AddScoped<ISoftDeleteLaunchHandler, SoftDeleteLaunchHandler>();
            services.AddScoped<IUpdateDataSetHandler, UpdateDataSetHandler>();
            services.AddScoped<IUpdateOneLaunchHandler, UpdateOneLaunchHandler>();

            return services;
        }

        public static IServiceCollection AddMediatR(this IServiceCollection services)
        {
            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
            return services;
        }
    }
}