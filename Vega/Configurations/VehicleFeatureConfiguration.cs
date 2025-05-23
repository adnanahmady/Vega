using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Vega.Core.Domain;

namespace Vega.Configurations;

public class VehicleFeatureConfiguration : IEntityTypeConfiguration<VehicleFeature>
{
    public void Configure(EntityTypeBuilder<VehicleFeature> builder)
    {
        builder.ToTable("VehicleFeatures");

        builder.Property(f => f.Name).IsRequired();
    }
}
