using Core.Domain.Entities;
using Infrastructure.Persistence.Context.Base;
using Infrastructure.Persistence.Context.Builder;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Context
{
    public class BaseApiContext : BaseContext
    {
        public BaseApiContext(DbContextOptions<BaseApiContext> options): base(options)
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ConfigurationModelBuilder());
            modelBuilder.ApplyConfiguration(new LaunchModelBuilder());
            modelBuilder.ApplyConfiguration(new LocationModelBuilder());
            modelBuilder.ApplyConfiguration(new MissionModelBuilder());
            modelBuilder.ApplyConfiguration(new PadModelBuilder());
            modelBuilder.ApplyConfiguration(new LaunchViewModelBuilder());
            
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            foreach(var entry in ChangeTracker.Entries<BaseEntity>())
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                    entry.Entity.UpdateDates();

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach(var entry in ChangeTracker.Entries<BaseEntity>())
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                    entry.Entity.UpdateDates();

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}