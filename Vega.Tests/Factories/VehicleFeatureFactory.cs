namespace Vega.Tests.Factories;

using Bogus;
using Models;

public class VehicleFeatureFactory
{
    public static VehicleFeature Create() =>
        new Faker<VehicleFeature>()
            .RuleFor(v => v.Name, f => f.PickRandom(
                "Wheel",
                "Glass",
                "Light"
            ))
            .Generate();
}
