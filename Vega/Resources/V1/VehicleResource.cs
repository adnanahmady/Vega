namespace Vega.Resources.V1;
public class VehicleResource
{
    public int Id { get; set; }

    public bool IsRegistered { get; set; }

    public required ContactResource Contact { get; set; }

    public required ModelResource Model { get; set; }

    public required MakeResource Make { get; set; }

    public ICollection<VehicleFeatureResource> VehicleFeatures { get; set; } =
        new List<VehicleFeatureResource>();

    public DateTime CreatedAt = DateTime.Now;

    public DateTime UpdatedAt = DateTime.Now;
}
