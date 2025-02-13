namespace Vega.Resources.V1;

public class MakeResource
{
    public int Id { get; set; }
    public string Name { get; set; }

    public List<ModelResource> Models { get; set; }
}
