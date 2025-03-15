using Vega.Core.Domain;

namespace Vega.Tests.Factories;

using Bogus;

public class MakeFactory
{
    public static Make Create() => new Faker<Make>()
            .RuleFor(m => m.Name, f => f.Name.FindName())
            .Generate();
}
