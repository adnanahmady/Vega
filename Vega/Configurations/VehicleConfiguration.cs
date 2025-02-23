namespace Vega.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Models;

public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder
            .Property(v => v.ContactName)
            .HasMaxLength(255)
            .IsRequired();

        builder
            .Property(v => v.ContactEmail)
            .HasMaxLength(255)
            .IsRequired(false);

        builder
            .Property(v => v.ContactPhone)
            .IsRequired()
            .HasMaxLength(255);

        builder
            .HasOne(v => v.Model)
            .WithMany(m => m.Vehicles)
            .IsRequired()
            .HasForeignKey(v => v.ModelId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Ignore(v => v.VehicleFeatureIds);

        builder
            .HasMany(v => v.VehicleFeatures)
            .WithMany(vf => vf.Vehicles)
            .UsingEntity<Dictionary<string, object>>(
                "FeaturesForVehicles",
                j => j.HasOne<VehicleFeature>()
                    .WithMany()
                    .HasForeignKey("VehicleFeatureId"),
                j => j.HasOne<Vehicle>()
                    .WithMany()
                    .HasForeignKey("VehicleId"),
                j => j.HasKey(
                        "VehicleFeatureId",
                        "VehicleId"
                    )
                );
    }
}
