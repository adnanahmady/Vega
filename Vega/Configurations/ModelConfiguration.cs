namespace Vega.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

public class ModelConfiguration : IEntityTypeConfiguration<Model>
{
    public void Configure(EntityTypeBuilder<Model> builder)
    {
        builder
            .Property(m => m.Name)
            .HasMaxLength(255)
            .IsRequired();
    }
}
