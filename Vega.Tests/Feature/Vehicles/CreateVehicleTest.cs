namespace Vega.Tests.Feature.Vehicles;

using System.Net;
using System.Net.Http.Json;
using Domain;
using Factories;
using Microsoft.EntityFrameworkCore;
using Resources.V1;
using Shouldly;
using Support;

public class CreateVehicleTest : IClassFixture<TestableWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly VegaDbContext _context;

    public CreateVehicleTest(TestableWebApplicationFactory factory)
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
            })
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
    public async Task GivenWrongDataWhenCalledThenShouldThrowError(
        Func<Dictionary<string, object>, Dictionary<string, object>> fn)
    {
        this._context.Models.Add(ModelFactory.Create());
        this._context.VehicleFeatures.Add(VehicleFeatureFactory.Create());
        await this._context.SaveChangesAsync();
        var data = new Dictionary<string, object>()
        {
            { "IsRegistered", true },
            { "ContactName", "John" },
            { "ContactPhone", "09119933311" },
            { "ContactEmail", "user@dummy.com" },
            { "ModelId", this._context.Models.First().Id },
            { "VehicleFeatureIds", new []
            {
                this._context.VehicleFeatures.First().Id
            }},
        };
        data = fn(data);

        var response = await this._client.PostAsJsonAsync("/api/v1/vehicles", data);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GivenVehicleWhenCalledThenShouldStoreInDatabase()
    {
        this._context.Models.Add(ModelFactory.Create());
        this._context.VehicleFeatures.Add(VehicleFeatureFactory.Create());
        await this._context.SaveChangesAsync();
        var data = new
        {
            Id = 1000,
            IsRegistered = true,
            ContactName = "John",
            ContactPhone = "09119933311",
            ContactEmail = "user@dummy.com",
            ModelId = this._context.Models.First().Id,
            VehicleFeatureIds = new[]
            {
                this._context.VehicleFeatures.First().Id
            }
        };

        var response = await this._client.PostAsJsonAsync("/api/v1/vehicles", data);

        var vehicle = await response.Content.ReadFromJsonAsync<VehicleResource>();
        var exists = await this._context.Vehicles.AnyAsync(
            v => v.Id == vehicle.Id
                 && v.ContactEmail == data.ContactEmail
                 && v.ContactPhone == data.ContactPhone);
        exists.ShouldBeTrue();
    }

    [Fact]
    public async Task GivenVehicleWhenCalledThenShouldResponseExpectedFields()
    {
        this._context.Models.Add(ModelFactory.Create());
        this._context.VehicleFeatures.Add(VehicleFeatureFactory.Create());
        await this._context.SaveChangesAsync();
        var data = new
        {
            Id = 1000,
            IsRegistered = true,
            ContactName = "John",
            ContactPhone = "09119933311",
            ContactEmail = "user@dummy.com",
            ModelId = this._context.Models.First().Id,
            VehicleFeatureIds = new[]
            {
                this._context.VehicleFeatures.First().Id
            }
        };

        var response = await this._client.PostAsJsonAsync("/api/v1/vehicles", data);

        var vehicle = await response.Content.ReadFromJsonAsync<VehicleResource>();
        vehicle.Id.ShouldBeGreaterThan(0);
        vehicle.Id.ShouldBeLessThan(1000);
        vehicle.IsRegistered.ShouldBe(data.IsRegistered);
        vehicle.ContactName.ShouldBe(data.ContactName);
        vehicle.ContactPhone.ShouldBe(data.ContactPhone);
        vehicle.ContactEmail.ShouldBe(data.ContactEmail);
    }

    [Fact]
    public async Task GivenCreateWhenServiceCalledThenVehicleShouldGetCreated()
    {
        this._context.Models.Add(ModelFactory.Create());
        this._context.VehicleFeatures.Add(VehicleFeatureFactory.Create());
        this._context.SaveChanges();
        var data = new
        {
            Id = 1000,
            IsRegistered = true,
            ContactName = "John",
            ContactPhone = "09119933134",
            ContactEmail = "admin@gmail.com",
            ModelId = this._context.Models.First().Id,
            VehicleFeatureIds = new[]
            {
                this._context.VehicleFeatures.First().Id
            }
        };

        var response = await this._client.PostAsJsonAsync("/api/v1/vehicles", data);

        response.StatusCode.ShouldBe(HttpStatusCode.Created);
    }
}
