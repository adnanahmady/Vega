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
}
