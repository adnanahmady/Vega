namespace Vega.Core.Domain;

public class VehiclePhoto
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public required Vehicle Vehicle { get; set; }
    public int VehicleId { get; set; }
}
