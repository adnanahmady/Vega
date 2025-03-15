namespace Vega.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Vega.Core.Domain;

public class ModelConfiguration : IEntityTypeConfiguration<Model>
{
    public void Configure(EntityTypeBuilder<Model> builder) => builder
            .Property(m => m.Name)
            .HasMaxLength(255)
            .IsRequired();
}
