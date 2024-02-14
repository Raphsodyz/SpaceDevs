using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Context.Builder
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