using Vega.Core.Domain;

namespace Vega.Tests.Factories;

using Bogus;

public static class VehicleFactory
{
    public static Vehicle Create() => Create(
        (f, v) => f.Name.FullName(),
        (f, v) => f.Internet.Email(),
        (f, v) => f.Phone.PhoneNumber(),
        (f, v) => ModelFactory.Create(),
        (f, v) => new[] { VehicleFeatureFactory.Create() },
        (f, v) => f.Random.Bool()
    );

    public static Vehicle Create(Func<Faker, Vehicle, object> model) => Create(
        (f, v) => f.Name.FullName(),
        (f, v) => f.Internet.Email(),
        (f, v) => f.Phone.PhoneNumber(),
        model,
        (f, v) => new[] { VehicleFeatureFactory.Create() },
        (f, v) => f.Random.Bool()
    );

    public static Vehicle Create(
        Func<Faker, Vehicle, object> contactName,
        Func<Faker, Vehicle, object> model
    ) => Create(
        contactName,
        (f, v) => f.Internet.Email(),
        (f, v) => f.Phone.PhoneNumber(),
        model,
        (f, v) => new[] { VehicleFeatureFactory.Create() },
        (f, v) => f.Random.Bool()
    );

    public static Vehicle Create(
        Func<Faker, Vehicle, object> cn,
        Func<Faker, Vehicle, object> ce,
        Func<Faker, Vehicle, object> cp,
        Func<Faker, Vehicle, object> model,
        Func<Faker, Vehicle, object> features,
        Func<Faker, Vehicle, bool> registered
    ) => new Faker<Vehicle>()
        .RuleFor(v => v.ContactName, cn)
        .RuleFor(v => v.ContactEmail, ce)
        .RuleFor(v => v.ContactPhone, cp)
        .RuleFor(v => v.Model, model)
        .RuleFor(v => v.VehicleFeatures, features)
        .RuleFor(v => v.IsRegistered, registered)
        .Generate();
}
