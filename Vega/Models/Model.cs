using System.ComponentModel.DataAnnotations;

namespace Vega.Models;

public class Model
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public Make Make { get; set; }
}
