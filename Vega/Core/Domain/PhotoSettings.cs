namespace Vega.Core.Domain;

public class PhotoSettings
{
    public int MaxBytes { get; set; }
    public string[] AcceptedFileTypes { get; set; } = new string[] { };

    public bool IsSupported(string fileName) => AcceptedFileTypes.Any(
        s => s == Path.GetExtension(fileName.ToLower())
    );
}
