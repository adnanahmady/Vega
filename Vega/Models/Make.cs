using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Vega.Models;

using System.Collections.ObjectModel;

public class Make
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public ICollection<Model> Models { get; set; } = new Collection<Model>();
}
