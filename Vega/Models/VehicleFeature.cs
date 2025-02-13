namespace Vega.Models;

public class VehicleFeature
{
    public int Id { get; set; }

    public string Name { get; set; }

    public ICollection<Vehicle> Vehicles { get; set; }
}
