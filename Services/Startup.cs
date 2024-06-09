using Application.Handlers.CommandHandlers.LaunchApi;
using Application.Handlers.QueryHandlers.LaunchApi;
using Domain.ExternalServices;
using Domain.Handlers;
using Domain.Interface;
using Infrastructure.ExternalServices;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Repository;
using MediatR;

namespace Services
{
    public class Startup : IStartup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(cfg => {
                cfg.EnableAnnotations();
            });

            services.AddDbContext<FutureSpaceContext>();

            //Repository Dependencies ..
            services.AddTransient<IConfigurationRepository, ConfigurationRepository>();
            services.AddTransient<IGenericDapperRepository, GenericDapperRepository>();
            services.AddTransient<ILaunchRepository, LaunchRepository>();
            services.AddTransient<ILaunchServiceProviderRepository, LaunchServiceProviderRepository>();
            services.AddTransient<ILaunchViewRepository, LaunchViewRepository>();
            services.AddTransient<ILocationRepository, LocationRepository>();
            services.AddTransient<IMissionRepository, MissionRepository>();
            services.AddTransient<IOrbitRepository, OrbitRepository>();
            services.AddTransient<IPadRepository, PadRepository>();
            services.AddTransient<IRocketRepository, RocketRepository>();
            services.AddTransient<IStatusRepository, StatusRepository>();
            services.AddTransient<IUpdateLogRepository, UpdateLogRepository>();

            //External Services Dependencies ..
            services.AddTransient<IRequestLaunchService, GetLaunchesFromSpaceDevs>();

            //Handler Dependencies ..
            services.AddTransient<IGetAllLaunchesPagedHandler, GetAllLaunchesPagedHandler>();
            services.AddTransient<IGetOneLaunchHandler, GetOneLaunchHandler>();
            services.AddTransient<ISearchByParamHandler, SearchByParamHandler>();
            services.AddTransient<ISoftDeleteLaunchHandler, SoftDeleteLaunchHandler>();
            services.AddTransient<IUpdateDataSetHandler, UpdateDataSetHandler>();
            services.AddTransient<IUpdateOneLaunchHandler, UpdateOneLaunchHandler>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

            services.AddHttpClient();
        }

        public void Configure(WebApplication app, IWebHostEnvironment environment)
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            app.MapControllers();
        }
    }

    public interface IStartup
    {
        IConfiguration Configuration { get; }
        void Configure(WebApplication app, IWebHostEnvironment enviroment);
        void ConfigureServices(IServiceCollection services);
    }

    public static class StartupExtensions
    {
        public static WebApplicationBuilder UseStartup<T>(this WebApplicationBuilder webApplicationBuilder) where T : IStartup
        {
            var startup = Activator.CreateInstance(typeof(T), webApplicationBuilder.Configuration) as IStartup;
            if (startup == null)
            {
                throw new ArgumentException("Startup class is not available.");
            }

            startup.ConfigureServices(webApplicationBuilder.Services);

            var app = webApplicationBuilder.Build();

            startup.Configure(app, app.Environment);
            app.Run();

            return webApplicationBuilder;
        }
    }
}
