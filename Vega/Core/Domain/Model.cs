using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Vega.Core.Domain;

public class Model
{
    public int Id { get; set; }

    [Required]
    public required string Name { get; set; }

    public required Make Make { get; set; }
    public int MakeId { get; set; }

    public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}
