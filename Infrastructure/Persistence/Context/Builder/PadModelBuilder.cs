using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Context.Builder
{
    public class PadModelBuilder : IEntityTypeConfiguration<Pad>
    {
        public void Configure(EntityTypeBuilder<Pad> builder)
        {
            builder.Property(e => e.Search)
                .HasComputedColumnSql("LOWER(name)");
        }
    }
}