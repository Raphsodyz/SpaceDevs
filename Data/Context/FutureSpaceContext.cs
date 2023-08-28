using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Data.Context
{
    public class FutureSpaceContext : DbContext
    {
        public DbSet<Launch> Launch { get; set; }
        public DbSet<Configuration> Configuration { get; set; }
        public DbSet<LaunchDesignator> LaunchDesignator { get; set; }
        public DbSet<LaunchServiceProvider> LaunchServiceProvider { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<Mission> Mission { get; set; }
        public DbSet<Orbit> Orbit { get; set; }
        public DbSet<Pad> Pad { get; set; }
        public DbSet<Rocket> Rocket { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<UpdateLog> UpdateLog { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

                var connection = configuration.GetSection("ConnectionStrings:default").Value;
                optionsBuilder.UseMySql(connection, ServerVersion.AutoDetect(connection));
            }
        }

        public override int SaveChanges()
        {
            foreach(var entry in ChangeTracker.Entries<BaseEntity>())
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                    entry.Entity.BeforeSave();

            return base.SaveChanges();
        }
    }
}
