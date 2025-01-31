namespace Vega.Resources.V1;

public class VehicleResource
{
    public int Id { get; set; }

    public bool IsRegistered { get; set; }

    public string ContactName { get; set; }

    public string? ContactPhone { get; set; }

    public string ContactEmail { get; set; }

    public ModelResource Model { get; set; }

    public VehicleFeatureResource? VehicleFeature { get; set; }

    public DateTime CreatedAt = DateTime.Now;

    public DateTime UpdatedAt = DateTime.Now;
}
