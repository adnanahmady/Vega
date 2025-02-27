using System.Text.Json;

namespace Vega.Core.Domain;

public class Vehicle
{
    public int Id { get; set; }

    public bool IsRegistered { get; set; }

    public required string ContactName { get; set; }

    public string? ContactPhone { get; set; }

    public required string ContactEmail { get; set; }

    public required Model Model { get; set; }
    public int ModelId { get; set; }

    public ICollection<VehicleFeature> VehicleFeatures { get; set; } = new List<VehicleFeature>();
    public ICollection<long> VehicleFeatureIds { get; set; } = new List<long>();

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime LastUpdate { get; set; } = DateTime.Now;

    public override string ToString() => JsonSerializer.Serialize(this);
}
