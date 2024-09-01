using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Context.Builder
{
    public class LaunchModelBuilder : IEntityTypeConfiguration<Launch>
    {
        public void Configure(EntityTypeBuilder<Launch> builder)
        {
            builder.Property(e => e.Search)
                .HasComputedColumnSql("LOWER(name || ' ' || slug)");
        }
    }
}