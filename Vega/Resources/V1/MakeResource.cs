namespace Vega.Resources.V1;

public class MakeResource : KeyValuePairResource
{
    public List<KeyValuePairResource> Models { get; set; } = new List<KeyValuePairResource>();
}
