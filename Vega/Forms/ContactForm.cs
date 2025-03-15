using System.ComponentModel.DataAnnotations;

namespace Vega.Forms;

public class ContactForm
{
    [Required]
    public string Name { get; set; }

    [MaxLength(11)]
    public string? Phone { get; set; }

    [Required]
    public string Email { get; set; }
}
