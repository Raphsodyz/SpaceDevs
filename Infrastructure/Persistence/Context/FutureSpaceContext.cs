using Data.Context.Builder;
using Domain.Entities;
using Domain.Materializated.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence.Context
{
    public class FutureSpaceContext : DbContext
    {
        public FutureSpaceContext(DbContextOptions<FutureSpaceContext> options):base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
        
        public DbSet<Launch> Launch { get; set; }
        public DbSet<Configuration> Configuration { get; set; }
        public DbSet<LaunchServiceProvider> LaunchServiceProvider { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<Mission> Mission { get; set; }
        public DbSet<Orbit> Orbit { get; set; }
        public DbSet<Pad> Pad { get; set; }
        public DbSet<Rocket> Rocket { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<UpdateLog> UpdateLog { get; set; }
        public DbSet<LaunchView> LaunchView { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

                var connection = Environment.GetEnvironmentVariable(configuration.GetSection("ConnectionStrings:default").Value);
                optionsBuilder.UseNpgsql(connection);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(ConfigurationModelBuilderSingleton.GetInstance());
            modelBuilder.ApplyConfiguration(LaunchModelBuilderSingleton.GetInstance());
            modelBuilder.ApplyConfiguration(LaunchViewModelBuilderSingleton.GetInstance());
            modelBuilder.ApplyConfiguration(LocationModelBuilderSingleton.GetInstance());
            modelBuilder.ApplyConfiguration(MissionModelBuilderSingleton.GetInstance());
            modelBuilder.ApplyConfiguration(PadModelBuilderSingleton.GetInstance());
            
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            foreach(var entry in ChangeTracker.Entries<BaseEntity>())
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                    entry.Entity.BeforeSave();

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach(var entry in ChangeTracker.Entries<BaseEntity>())
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                    entry.Entity.BeforeSave();

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
