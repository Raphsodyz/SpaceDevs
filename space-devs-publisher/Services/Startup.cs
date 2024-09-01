using Infrastructure;

namespace Services
{
    public class Startup(IConfiguration configuration) : IStartup
    {
        public IConfiguration Configuration { get; } = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(cfg => {
                cfg.EnableAnnotations();
            });

            services
                .AddInfrastructure(Configuration.GetSection("ConnectionStrings:Postgresql").Value!);

            services.AddHttpClient();
        }

        public void Configure(WebApplication app, IWebHostEnvironment enviroment)
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
            var startup = Activator.CreateInstance(typeof(T), webApplicationBuilder.Configuration) as IStartup ?? throw new ArgumentException("Classe de início não está disponível.");
            startup.ConfigureServices(webApplicationBuilder.Services);

            var app = webApplicationBuilder.Build();

            startup.Configure(app, app.Environment);
            app.Run();

            return webApplicationBuilder;
        }
    }
}