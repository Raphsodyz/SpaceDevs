using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Context.Builder
{
    public class LocationModelBuilder : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.Property(e => e.Search)
                .HasComputedColumnSql("LOWER(name)");
        }
    }
}