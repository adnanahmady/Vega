using Bogus;

using Vega.Core.Domain;

namespace Vega.Tests.Factories;

public class VehiclePhotoFactory
{
    public static VehiclePhoto Create() =>
        new Faker<VehiclePhoto>()
            .RuleFor(photo => photo.Name, f => f.Name.FirstName())
            .RuleFor(photo => photo.Vehicle, f => VehicleFactory.Create())
            .Generate();
}
