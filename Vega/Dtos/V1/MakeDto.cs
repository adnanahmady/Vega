using System.ComponentModel.DataAnnotations;
using Vega.Models;

namespace Vega.Dtos.V1;

public class MakeDto
{
    public int Id { get; set; }
    public string Name { get; set; }

    public List<ModelDto> Models { get; set; }
}