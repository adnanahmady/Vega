using System.ComponentModel.DataAnnotations;

namespace Vega.Forms;

public class ContactForm
{
    [Required]
    [MinLength(2)]
    public string? Name { get; set; }

    [MaxLength(11)]
    public string? Phone { get; set; }

    [Required]
    [MinLength(2)]
    public string? Email { get; set; }
}
