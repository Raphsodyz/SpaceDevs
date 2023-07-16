using Business.Business;
using Business.Interface;
using Data.Context;
using Data.Interface;
using Data.Repository;

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
            services.AddSwaggerGen();
            services.AddDbContext<FutureSpaceContext>();

            services.AddCors();
            
            services.AddTransient<IConfigurationRepository, ConfigurationRepository>();
            services.AddTransient<ILaunchDesignatorRepository, LaunchDesignatorRepository>();
            services.AddTransient<ILaunchRepository, LaunchRepository>();
            services.AddTransient<ILaunchServiceProviderRepository, LaunchServiceProviderRepository>();
            services.AddTransient<ILocationRepository, LocationRepository>();
            services.AddTransient<IPadRepository, PadRepository>();
            services.AddTransient<IRocketRepository, RocketRepository>();
            services.AddTransient<IStatusRepository, StatusRepository>();

            services.AddTransient<IConfigurationBusiness, ConfigurationBusiness>();
            services.AddTransient<ILaunchDesignatorBusiness, LaunchDesignatorBusiness>();
            services.AddTransient<ILaunchBusiness, LaunchBusiness>();
            services.AddTransient<ILaunchServiceProviderBusiness, LaunchServiceProviderBusiness>();
            services.AddTransient<ILocationBusiness, LocationBusiness>();
            services.AddTransient<IPadBusiness, PadBusiness>();
            services.AddTransient<IRocketBusiness, RocketBusiness>();
            services.AddTransient<IStatusBusiness, StatusBusiness>();
            
            services.AddTransient<ILaunchApiBusiness, LaunchApiBusiness>();
            services.AddTransient<IJobBusiness, JobBusiness>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        public void Configure(WebApplication app, IWebHostEnvironment environment)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(a => a.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHttpsRedirection();
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
