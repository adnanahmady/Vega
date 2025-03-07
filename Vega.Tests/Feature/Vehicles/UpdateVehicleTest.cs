using System.Text.Json;

using Microsoft.EntityFrameworkCore;

using Vega.Core.Domain;
using Vega.Persistence;

namespace Vega.Tests.Feature.Vehicles;

using System.Net;
using System.Net.Http.Json;

using Factories;

using Resources.V1;

using Shouldly;

using Support;

public class UpdateVehicleTest : IClassFixture<TestableWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly VegaDbContext _context;

    public UpdateVehicleTest(TestableWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _context = factory.ResolveDbContext<VegaDbContext>();
    }

    public static IEnumerable<object[]> InvalidDataForValidationTest()
    {
        yield return new object[]
        {
            new Func<Dictionary<string, object>, Dictionary<string, object>>(data =>
            {
                ((Dictionary<string, object>)data["Contact"]).Remove("Name");

                return data;
            }),
            1,
            "Contact.Name"
        };

        yield return new object[]
        {
            new Func<Dictionary<string, object>, Dictionary<string, object>>(data =>
            {
                (
                    (Dictionary<string, object>)data["Contact"]
                )["Phone"] = "1".PadRight(12, '1');

                return data;
            }),
            1,
            "Contact.Phone"
        };

        yield return new object[]
        {
            new Func<Dictionary<string, object>, Dictionary<string, object>>(data =>
            {
                ((Dictionary<string, object>)data["Contact"]).Remove("Email");

                return data;
            }),
            1,
            "Contact.Email"
        };

        yield return new object[]
        {
            new Func<Dictionary<string, object>, Dictionary<string, object>>(data =>
            {
                data.Remove("ModelId");

                return data;
            }),
            1,
            "ModelId"
        };

        yield return new object[]
        {
            new Func<Dictionary<string, object>, Dictionary<string, object>>(data =>
            {
                data.Remove("FeatureIds");

                return data;
            }),
            1,
            "FeatureIds"
        };
    }

    [Theory]
    [MemberData(nameof(InvalidDataForValidationTest))]
    public async Task GivenWrongDataWhenCalledThenShouldReturnBadRequest(
        Func<Dictionary<string, object>, Dictionary<string, object>> fn,
        int expectedCount,
        string expectedField)
    {
        // Arrange
        var (url, vehicle) = await Prepare();
        var data = new Dictionary<string, object>()
        {
            { "IsRegistered", vehicle.IsRegistered },
            { "ModelId", vehicle.ModelId },
            { "FeatureIds", vehicle.VehicleFeatures.Select(
                vf => vf.Id).ToList() },
            { "Contact", new Dictionary<string, object>() {
                { "Name", vehicle.ContactName },
                { "Email", vehicle.ContactEmail },
                { "Phone", "09117773313" },
            }}
        };
        data = fn(data);

        // Act
        var response = await _client.PutAsJsonAsync(url, data);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var content = await response.Content.ReadFromJsonAsync<JsonElement>();
        content.GetProperty("errors").EnumerateArray()
            .Count(i => i.GetProperty("field").GetString() == expectedField)
            .ShouldBe(1);
        content.GetProperty("errors").GetArrayLength().ShouldBe(expectedCount);
    }

    [Fact]
    public async Task GivenVehicleDataWhenCalledThenShouldStoreCorrectlyInDatabase()
    {
        var (url, vehicle) = await Prepare();
        var data = new
        {
            vehicle.IsRegistered,
            vehicle.ModelId,
            FeatureIds = vehicle.VehicleFeatures.Select(vf => vf.Id),
            Contact = new
            {
                Name = vehicle.ContactName,
                Email = vehicle.ContactEmail,
                Phone = "09117773313",
            },
        };

        await _client.PutAsJsonAsync(url, data);

        var count = await _context.Vehicles
            .Where(v => v.Id == vehicle.Id)
            .Where(v => v.IsRegistered == data.IsRegistered)
            .Where(v => v.ModelId == data.ModelId)
            .Where(v => v.VehicleFeatures.All(
                vf => data.FeatureIds.Contains(vf.Id)))
            .Where(v => v.ContactEmail == data.Contact.Email)
            .Where(v => v.ContactName == data.Contact.Name)
            .Where(v => v.ContactPhone == data.Contact.Phone)
            .CountAsync();
        count.ShouldBe(1);
    }

    [Fact]
    public async Task GivenVehicleDataWhenCalledThenResponseWithExpectedData()
    {
        var (url, vehicle) = await Prepare();
        var feature = await FactoryFeature();
        var data = new
        {
            vehicle.IsRegistered,
            vehicle.ModelId,
            FeatureIds = new[] { feature.Id },
            Contact = new
            {
                Name = vehicle.ContactName,
                Email = vehicle.ContactEmail,
                Phone = "09117773313",
            },
        };

        var response = await _client.PutAsJsonAsync(url, data);
        var resource = await response.Content.ReadFromJsonAsync<VehicleResource>();

        resource!.IsRegistered.ShouldBe(data.IsRegistered);
        resource.Model.Id.ShouldBe(data.ModelId);
        resource.Contact.Email.ShouldBe(data.Contact.Email);
        resource.Contact.Name.ShouldBe(data.Contact.Name);
        resource.Contact.Phone.ShouldBe(data.Contact.Phone);
        resource.VehicleFeatures.ShouldBeOfType<List<KeyValuePairResource>>();
        resource.VehicleFeatures.OrderBy(i => i.Id).First().Id.ShouldBe(feature.Id);
    }

    [Fact]
    public async Task GivenVehicleIdWhenCalledThenResponseOk()
    {
        var (url, vehicle) = await Prepare();
        var feature = await FactoryFeature();
        var data = new
        {
            vehicle.IsRegistered,
            vehicle.ModelId,
            FeatureIds = new[] { feature.Id },
            Contact = new
            {
                Name = vehicle.ContactName,
                Email = vehicle.ContactEmail,
                Phone = "09117773313",
            },
        };

        var response = await _client.PutAsJsonAsync(url, data);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    private async Task<VehicleFeature> FactoryFeature()
    {
        var feature = VehicleFeatureFactory.Create();
        _context.VehicleFeatures.Add(feature);
        await _context.SaveChangesAsync();
        return feature;
    }

    private async Task<(string url, Vehicle vehicle)> Prepare()
    {
        var vehicle = await FactoryVehicle();
        var url = $"/api/v1/vehicles/{vehicle.Id}";
        return (url, vehicle);
    }

    private async Task<Vehicle> FactoryVehicle()
    {
        var vehicle = VehicleFactory.Create();
        _context.Vehicles.Add(vehicle);
        await _context.SaveChangesAsync();

        return vehicle;
    }
}
