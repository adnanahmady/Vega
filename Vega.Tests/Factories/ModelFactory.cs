using Vega.Core.Domain;

namespace Vega.Tests.Factories;

using Bogus;

public static class ModelFactory
{
    public static Model Create() => Create(
        (f, v) => f.Name.FullName(),
        (f, v) => MakeFactory.Create()
    );

    public static Model Create(
        Func<Faker, Model, object> name,
        Func<Faker, Model, object> make) => new Faker<Model>()
            .RuleFor(m => m.Name, name)
            .RuleFor(m => m.Make, make)
            .Generate();
}
