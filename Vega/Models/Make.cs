using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Vega.Models;

public class Make
{
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }

    public List<Model> Models { get; set; }
}