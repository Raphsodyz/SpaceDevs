using Core.Domain.Entities;
using Infrastructure.Persistence.Context.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Context
{
    public class BaseApiContext : BaseContext
    {
        public BaseApiContext(DbContextOptions<BaseApiContext> options): base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        //Colocar DbSets

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            foreach(var entry in ChangeTracker.Entries<EntidadeBase>())
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                    entry.Entity.AtualizaDatas();

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach(var entry in ChangeTracker.Entries<EntidadeBase>())
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                    entry.Entity.AtualizaDatas();

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}