namespace Vega.Validations;

using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class RegexCollectionAttribute : ValidationAttribute
{
    private readonly string _pattern;

    public RegexCollectionAttribute(string pattern) => _pattern = pattern;

    public override bool IsValid(object? value)
    {
        if (value is IEnumerable collection)
        {
            foreach (var item in collection)
            {
                if (item is not int || !Regex.IsMatch(item.ToString() ?? string.Empty, _pattern))
                {
                    return false;
                }
            }

            return true;
        }

        return false;
    }
}
