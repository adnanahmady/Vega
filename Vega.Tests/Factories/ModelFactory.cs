using Vega.Core.Domain;

namespace Vega.Tests.Factories;

using Bogus;

public class ModelFactory
{
    public static Model Create() => new Faker<Model>()
            .RuleFor(m => m.Name, f => f.Name.FindName())
            .RuleFor(m => m.Make, f => MakeFactory.Create())
            .Generate();
}
