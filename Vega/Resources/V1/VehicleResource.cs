namespace Vega.Resources.V1;
public class VehicleResource
{
    public int Id { get; set; }

    public bool IsRegistered { get; set; }

    public required ContactResource Contact { get; set; }

    public required KeyValuePairResource Model { get; set; }

    public required KeyValuePairResource Make { get; set; }

    public ICollection<KeyValuePairResource> VehicleFeatures { get; set; } =
        new List<KeyValuePairResource>();

    public DateTime CreatedAt = DateTime.Now;

    public DateTime UpdatedAt = DateTime.Now;
}
