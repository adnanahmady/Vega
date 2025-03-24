using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Vega.Core.Domain;

namespace Vega.Configurations;

public class VehiclePhotoConfiguration : IEntityTypeConfiguration<VehiclePhoto>
{
    public void Configure(EntityTypeBuilder<VehiclePhoto> builder)
    {
        builder
            .Property(p => p.Name)
            .HasMaxLength(255)
            .IsRequired();

        builder
            .HasOne(p => p.Vehicle)
            .WithMany(v => v.Photos)
            .HasForeignKey(p => p.VehicleId);
    }
}
