namespace Vega.Forms;

using System.ComponentModel.DataAnnotations;

public class CreateVehicleForm
{
    public bool IsRegistered { get; set; }

    [Required]
    public string ContactName { get; set; }

    [MaxLength(11)]
    public string? ContactPhone { get; set; }

    [Required]
    public string ContactEmail { get; set; }

    [Required]
    [RegularExpression(@"^[1-9]+\d*$", ErrorMessage = "Id must be a valid integer.")]
    public int ModelId { get; set; }

    [Required]
    [RegularExpression(@"^[1-9]+\d*$", ErrorMessage = "Id must be a valid integer.")]
    public int VehicleFeatureId { get; set; }
}
