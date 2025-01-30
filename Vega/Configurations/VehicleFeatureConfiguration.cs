using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vega.Models;

namespace Vega.Configurations;

public class VehicleFeatureConfiguration : IEntityTypeConfiguration<VehicleFeature>
{
    public void Configure(EntityTypeBuilder<VehicleFeature> builder)
    {
        builder.ToTable("VehicleFeatures");

        builder.Property(f => f.Name).IsRequired();
    }
}
