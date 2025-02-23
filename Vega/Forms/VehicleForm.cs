using System.Text.Json;

namespace Vega.Forms;

using System.ComponentModel.DataAnnotations;

using Validations;

public class VehicleForm
{
    public bool IsRegistered { get; set; }

    public ContactForm Contact { get; set; }

    [Required]
    [RegularExpression(@"^[1-9]+\d*$", ErrorMessage = "Id must be a valid integer.")]
    public int ModelId { get; set; }

    [Required]
    [RegexCollection(@"^[1-9]+\d*$", ErrorMessage = "Id must be a valid integer.")]
    public int[] FeatureIds { get; set; }

    public override string ToString() => JsonSerializer.Serialize(this);
}
