using Vega.Core.Domain;

namespace Vega.Tests.Factories;

using Bogus;

public class VehicleFactory
{
    public static Vehicle Create() => new Faker<Vehicle>()
            .RuleFor(v => v.ContactName, f => f.Name.FullName())
            .RuleFor(v => v.ContactEmail, f => f.Internet.Email())
            .RuleFor(v => v.ContactPhone, f => f.Phone.PhoneNumber())
            .RuleFor(v => v.Model, f => ModelFactory.Create())
            .RuleFor(v => v.VehicleFeatures, f => new[]
            {
                VehicleFeatureFactory.Create()
            })
            .RuleFor(v => v.IsRegistered, f => f.Random.Bool())
            .Generate();
}
