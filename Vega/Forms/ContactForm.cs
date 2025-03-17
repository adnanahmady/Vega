using System.ComponentModel.DataAnnotations;

namespace Vega.Forms;

public class ContactForm
{
    [Required]
    public required string Name { get; set; }

    [MaxLength(11)]
    public string? Phone { get; set; }

    [Required]
    public required string Email { get; set; }
}
