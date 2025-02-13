using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Vega.Models;

namespace Vega.Configurations;

public class MakeConfiguration : IEntityTypeConfiguration<Make>
{
    public void Configure(EntityTypeBuilder<Make> builder)
    {
        builder
            .HasMany(m => m.Models)
            .WithOne(m => m.Make)
            .IsRequired();

        builder
            .Property(m => m.Name)
            .HasMaxLength(255)
            .IsRequired();
    }
}
