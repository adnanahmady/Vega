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

        switch (base64.Length % 4)
        {
            case 2: return base64 + "==";
            case 3: return base64 + "=";
            default: return base64;
        }
    }
}
