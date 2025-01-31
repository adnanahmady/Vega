namespace Vega.Models;

public class Vehicle
{
    public int Id { get; set; }

    public bool IsRegistered { get; set; }

    public string ContactName { get; set; }

    public string? ContactPhone { get; set; }

    public string ContactEmail { get; set; }

    public Model Model { get; set; }
    public int ModelId { get; set; }

    public VehicleFeature? VehicleFeature { get; set; }
    public int? VehicleFeatureId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime LastUpdate { get; set; } = DateTime.Now;
}
