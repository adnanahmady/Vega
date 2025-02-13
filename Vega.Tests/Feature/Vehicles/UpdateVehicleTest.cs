namespace Vega.Tests.Feature.Vehicles;

using System.Net;
using System.Net.Http.Json;
using System.Security.Policy;
using System.Text.Json;
using Bogus.DataSets;
using Domain;
using Factories;
using Microsoft.EntityFrameworkCore;
using Models;
using Newtonsoft.Json;
using Resources.V1;
using Shouldly;
using Support;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Vehicle = Models.Vehicle;

public class UpdateVehicleTest : IClassFixture<TestableWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly VegaDbContext _context;

    public UpdateVehicleTest(TestableWebApplicationFactory factory)
    {
        this._client = factory.CreateClient();
        this._context = factory.ResolveDbContext<VegaDbContext>();
    }

    public static IEnumerable<object[]> InvalidDataForValidationTest()
    {
        yield return new object[]
        {
            new Func<Dictionary<string, object>, Dictionary<string, object>>(data =>
            {
                data.Remove("ContactName");

                return data;
            }),
        };

        yield return new object[]
        {
            new Func<Dictionary<string, object>, Dictionary<string, object>>(data =>
            {
                data.Remove("ContactPhone");
                data.Add("ContactPhone", "1".PadRight(12, '1'));

                return data;
            })
        };

        yield return new object[]
        {
            new Func<Dictionary<string, object>, Dictionary<string, object>>(data =>
            {
                data.Remove("ContactEmail");

                return data;
            })
        };

        yield return new object[]
        {
            new Func<Dictionary<string, object>, Dictionary<string, object>>(data =>
            {
                data.Remove("ModelId");

                return data;
            })
        };

        yield return new object[]
        {
            new Func<Dictionary<string, object>, Dictionary<string, object>>(data =>
            {
                data.Remove("VehicleFeatureIds");

                return data;
            })
        };
    }

    [Theory]
    [MemberData(nameof(InvalidDataForValidationTest))]
    public async Task GivenWrongDataWhenCalledThenShouldReturnBadRequest(
        Func<Dictionary<string, object>, Dictionary<string, object>> fn)
    {
        var (url, vehicle) = await this.Prepare();
        var data = new Dictionary<string, object>()
        {
            { "IsRegistered", vehicle.IsRegistered },
            { "ModelId", vehicle.ModelId },
            { "VehicleFeatureIds", vehicle.VehicleFeatureIds },
            { "ContactName", vehicle.ContactName },
            { "ContactEmail", vehicle.ContactEmail },
            { "ContactPhone", "09117773313" },
        };
        data = fn(data);

        var response = await this._client.PutAsJsonAsync(url, data);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GivenVehicleDataWhenCalledThenShouldStoreCorrectlyInDatabase()
    {
        var (url, vehicle) = await this.Prepare();
        var data = new
        {
            IsRegistered = vehicle.IsRegistered,
            ModelId = vehicle.ModelId,
            VehicleFeatureIds = vehicle.VehicleFeatures.Select(vf => vf.Id),
            ContactName = vehicle.ContactName,
            ContactEmail = vehicle.ContactEmail,
            ContactPhone = "09117773313",
        };

        await this._client.PutAsJsonAsync(url, data);

        var count = await this._context.Vehicles
            .Where(v => v.Id == vehicle.Id)
            .Where(v => v.IsRegistered == data.IsRegistered)
            .Where(v => v.ModelId == data.ModelId)
            .Where(v => v.VehicleFeatures.All(
                vf => data.VehicleFeatureIds.Contains(vf.Id)))
            .Where(v => v.ContactEmail == data.ContactEmail)
            .Where(v => v.ContactName == data.ContactName)
            .Where(v => v.ContactPhone == data.ContactPhone)
            .CountAsync();
        count.ShouldBe(1);
    }

    [Fact]
    public async Task GivenVehicleDataWhenCalledThenResponseWithExpectedData()
    {
        var (url, vehicle) = await this.Prepare();
        var feature = await this.FactoryFeature();
        var data = new
        {
            IsRegistered = vehicle.IsRegistered,
            ModelId = vehicle.ModelId,
            VehicleFeatureIds = new[] { feature.Id },
            ContactName = vehicle.ContactName,
            ContactEmail = vehicle.ContactEmail,
            ContactPhone = "09117773313",
        };

        var response = await this._client.PutAsJsonAsync(url, data);
        var resource = await response.Content.ReadFromJsonAsync<VehicleResource>();

        resource.IsRegistered.ShouldBe(data.IsRegistered);
        resource.Model.Id.ShouldBe(data.ModelId);
        resource.ContactEmail.ShouldBe(data.ContactEmail);
        resource.ContactName.ShouldBe(data.ContactName);
        resource.ContactPhone.ShouldBe(data.ContactPhone);
        resource.VehicleFeatures.ShouldBeOfType<List<VehicleFeatureResource>>();
        resource.VehicleFeatures.First().Id.ShouldBe(feature.Id);
    }

    [Fact]
    public async Task GivenVehicleIdWhenCalledThenResponseOk()
    {
        var (url, vehicle) = await this.Prepare();
        var feature = await this.FactoryFeature();
        var data = new
        {
            IsRegistered = vehicle.IsRegistered,
            ModelId = vehicle.ModelId,
            VehicleFeatureIds = new[] { feature.Id },
            ContactName = vehicle.ContactName,
            ContactEmail = vehicle.ContactEmail,
            ContactPhone = "09117773313",
        };

        var response = await this._client.PutAsJsonAsync(url, data);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    private async Task<VehicleFeature> FactoryFeature()
    {
        var feature = VehicleFeatureFactory.Create();
        this._context.VehicleFeatures.Add(feature);
        await this._context.SaveChangesAsync();
        return feature;
    }

    private async Task<(string url, Vehicle vehicle)> Prepare()
    {
        var vehicle = await this.FactoryVehicle();
        var url = $"/api/v1/vehicles/{vehicle.Id}";
        return (url, vehicle);
    }

    private async Task<Vehicle> FactoryVehicle()
    {
        var vehicle = VehicleFactory.Create();
        this._context.Vehicles.Add(vehicle);
        await this._context.SaveChangesAsync();

        return vehicle;
    }
}
