using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Context.Builder
{
    public class MissionModelBuilder : IEntityTypeConfiguration<Mission>
    {
        public void Configure(EntityTypeBuilder<Mission> builder)
        {
            builder.Property(e => e.Search)
                .HasComputedColumnSql("LOWER(name)");
        }
    }
}