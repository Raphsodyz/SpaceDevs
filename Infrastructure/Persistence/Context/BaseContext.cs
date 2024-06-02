using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Persistence.Context
{
    public abstract class BaseContext : DbContext
    {
        private const string collation = "C";
        protected BaseContext(DbContextOptions options):base(options)
        {
            ChangeTracker.LazyLoadingEnabled = false;
            ChangeTracker.CascadeDeleteTiming = CascadeTiming.OnSaveChanges;
            ChangeTracker.DeleteOrphansTiming = CascadeTiming.OnSaveChanges;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .UseCollation(collation);

            base.OnModelCreating(modelBuilder);
        }
    }
}