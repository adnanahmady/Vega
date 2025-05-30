using Microsoft.EntityFrameworkCore;

using Vega.Configurations;
using Vega.Core.Domain;

namespace Vega.Persistence;

public class VegaDbContext : DbContext
{
    public DbSet<Make> Makes { get; set; }
    public DbSet<Model> Models { get; set; }
    public DbSet<VehicleFeature> VehicleFeatures { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<VehiclePhoto> VehiclePhotos { get; set; }

    public VegaDbContext(DbContextOptions<VegaDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new MakeConfiguration());
        modelBuilder.ApplyConfiguration(new ModelConfiguration());
        modelBuilder.ApplyConfiguration(new VehicleFeatureConfiguration());
        modelBuilder.ApplyConfiguration(new VehicleConfiguration());
        modelBuilder.ApplyConfiguration(new VehiclePhotoConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}
