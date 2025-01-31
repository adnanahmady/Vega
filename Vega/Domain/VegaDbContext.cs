namespace Vega.Domain;

using Configurations;
using Microsoft.EntityFrameworkCore;
using Models;

public class VegaDbContext : DbContext
{
    public DbSet<Make> Makes { get; set; }
    public DbSet<Model> Models { get; set; }
    public DbSet<VehicleFeature> VehicleFeatures { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }

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

        base.OnModelCreating(modelBuilder);
    }
}
