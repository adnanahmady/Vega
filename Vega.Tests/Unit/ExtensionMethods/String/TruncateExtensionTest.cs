namespace Vega.Tests.Unit.ExtensionMethods.String;

using Shouldly;
using Vega.ExtensionMethods;

public class TruncateExtensionTest
{
    [Fact]
    public void TextShould()
    {
        var count = 5;

        var content = "This is very long text text blah blah blah...";

        content
            .Truncate(count)
            .Split(' ')
            .Length
            .ShouldBeEquivalentTo(count);
    }
}
