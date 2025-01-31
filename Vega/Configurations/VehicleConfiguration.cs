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
            .IsRequired();

        builder
            .Property(v => v.ContactPhone)
            .IsRequired(false)
            .HasMaxLength(255);

        builder
            .HasOne(v => v.Model)
            .WithMany(m => m.Vehicles)
            .IsRequired()
            .HasForeignKey(v => v.ModelId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(v => v.VehicleFeature)
            .WithMany(vf => vf.Vehicles)
            .IsRequired(false)
            .HasForeignKey(v => v.VehicleFeatureId);
    }
}
