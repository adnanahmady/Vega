using Microsoft.EntityFrameworkCore;
using Vega.Configurations;
using Vega.Models;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace Vega.Domain;

public class VegaDbContext : DbContext
{
    public DbSet<Make> Makes { get; set; }
    public DbSet<Model> Models { get; set; }
    public DbSet<VehicleFeature> VehicleFeatures { get; set; }

    public VegaDbContext(DbContextOptions<VegaDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new MakeConfiguration());
        modelBuilder.ApplyConfiguration(new VehicleFeatureConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}