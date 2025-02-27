using System.ComponentModel.DataAnnotations;

namespace Vega.Core.Domain;

using System.Collections.ObjectModel;

public class Make
{
    public int Id { get; set; }

    [Required]
    public required string Name { get; set; }

    public ICollection<Model> Models { get; set; } = new Collection<Model>();
}
