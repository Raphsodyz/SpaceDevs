using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Context.Builder
{
    public class ConfigurationModelBuilder : IEntityTypeConfiguration<Configuration>
    {
        public void Configure(EntityTypeBuilder<Configuration> builder)
        {
            builder.Property(e => e.Search)
                .HasComputedColumnSql("LOWER(name || ' ' || family)");
        }
    }
}