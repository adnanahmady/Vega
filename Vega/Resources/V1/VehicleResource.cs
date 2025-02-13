namespace Vega.Resources.V1;

using System.ComponentModel.DataAnnotations;

public class VehicleResource
{
    public int Id { get; set; }

    public bool IsRegistered { get; set; }

    [Required]
    public string ContactName { get; set; }

    [MaxLength(11)]
    public string? ContactPhone { get; set; }

    [Required]
    public string ContactEmail { get; set; }

    public ModelResource Model { get; set; }

    public ICollection<VehicleFeatureResource> VehicleFeatures { get; set; }

    public DateTime CreatedAt = DateTime.Now;

    public DateTime UpdatedAt = DateTime.Now;
}
