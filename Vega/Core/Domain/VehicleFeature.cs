namespace Vega.Core.Domain;

public class VehicleFeature
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}
