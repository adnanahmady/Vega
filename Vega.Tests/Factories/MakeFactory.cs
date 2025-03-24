using Vega.Core.Domain;

namespace Vega.Tests.Factories;

using Bogus;

public static class MakeFactory
{
    public static Make Create() => Create((f, v) => f.Name.FullName());

    public static Make Create(
        Func<Faker, Make, object> name) => new Faker<Make>()
            .RuleFor(m => m.Name, name)
            .Generate();
}

