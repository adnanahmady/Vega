namespace Vega.Tests.Factories;

using Bogus;
using Models;

public class VehicleFactory
{
    public static Vehicle Create() =>
        new Faker<Vehicle>()
            .RuleFor(v => v.ContactName, f => f.Name.FullName())
            .RuleFor(v => v.ContactEmail, f => f.Internet.Email())
            .RuleFor(v => v.ContactPhone, f => f.Phone.PhoneNumber())
            .RuleFor(v => v.Model, f => ModelFactory.Create())
            .RuleFor(v => v.VehicleFeature, f => VehicleFeatureFactory.Create())
            .RuleFor(v => v.IsRegistered, f => f.Random.Bool())
            .Generate();
}
