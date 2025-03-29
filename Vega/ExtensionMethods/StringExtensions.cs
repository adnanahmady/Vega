namespace Vega.ExtensionMethods;

public static class StringExtensions
{
    public static string Truncate(this string str, int numberOfWords)
    {
        if (numberOfWords < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(numberOfWords),
                "numberOfWords should be greater than or equal to 0.");
        }

        if (numberOfWords == 0)
        {
            return "";
        }

        var words = str.Split(' ');

        if (words.Length <= numberOfWords)
        {
            return str;
        }

        return string.Join(' ', words.Take(numberOfWords));
    }

    public static string PadBase64(this string base64)
    {
        base64 = base64
            .Replace('-', '+')
            .Replace('_', '/');

        return (base64.Length % 4) switch
        {
            2 => base64 + "==",
            3 => base64 + "=",
            _ => base64,
        };
    }
}
