using System.Text.Json;

using Vega.Persistence;

namespace Vega.Tests.Feature.Vehicles;

using System.Net;
using System.Net.Http.Json;

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
        _client = factory.CreateClient();
        _context = factory.ResolveDbContext<VegaDbContext>();
    }

    public static IEnumerable<object[]> InvalidDataForValidationTest()
    {
        yield return new object[]
        {
            new Func<Dictionary<string, object>, Dictionary<string, object>>(data =>
            {
                (
                    (Dictionary<string, object>)data["Contact"]
                ).Remove("Name");

                return data;
            }),
            "Contact.Name",
            1
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
            "Contact.Phone",
            1
        };

        yield return new object[]
        {
            new Func<Dictionary<string, object>, Dictionary<string, object>>(data =>
            {
                (
                    (Dictionary<string, object>)data["Contact"]
                ).Remove("Email");

                return data;
            }),
            "Contact.Email",
            1
        };

        // ModelId should exist in the system
        yield return new object[]
        {
            new Func<Dictionary<string, object>, Dictionary<string, object>>(data =>
            {
                data["ModelId"] = 9999;

                return data;
            }),
            "ModelId",
            1
        };

        yield return new object[]
        {
            new Func<Dictionary<string, object>, Dictionary<string, object>>(data =>
            {
                data.Remove("ModelId");

                return data;
            }),
            "ModelId",
            1
        };

        yield return new object[]
        {
            new Func<Dictionary<string, object>, Dictionary<string, object>>(data =>
            {
                data.Remove("FeatureIds");

                return data;
            }),
            "FeatureIds",
            1
        };
    }

    [Theory]
    [MemberData(nameof(InvalidDataForValidationTest))]
    public async Task GivenWrongDataWhenCalledThenShouldThrowError(
        Func<Dictionary<string, object>, Dictionary<string, object>> fn,
        string expectedField,
        int expectedCount)
    {
        // Arrange
        _context.Models.Add(ModelFactory.Create());
        _context.VehicleFeatures.Add(VehicleFeatureFactory.Create());
        await _context.SaveChangesAsync();
        var data = new Dictionary<string, object>()
        {
            { "IsRegistered", true },
            {
                "Contact", new Dictionary<string, object>
                {
                    { "Name", "John" },
                    { "Phone", "09119933311" },
                    { "Email", "user@dummy.com" },
                }
            },
            { "ModelId", _context.Models.OrderBy(i => i.Id).First().Id },
            { "FeatureIds", new[]
                {
                    _context.VehicleFeatures.OrderBy(i => i.Id).First().Id
                }},
        };
        data = fn(data);

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/vehicles", data);

        // Arrange
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var content = await response.Content.ReadFromJsonAsync<JsonElement>();
        content.GetProperty("errors").EnumerateArray()
            .Count(i => i.GetProperty("field").GetString() == expectedField)
            .ShouldBe(1);
        content.GetProperty("errors").GetArrayLength().ShouldBe(expectedCount);
    }

    [Fact]
    public async Task GivenVehicleWhenCalledThenShouldStoreInDatabase()
    {
        _context.Models.Add(ModelFactory.Create());
        _context.VehicleFeatures.Add(VehicleFeatureFactory.Create());
        await _context.SaveChangesAsync();
        var data = new
        {
            Id = 1000,
            IsRegistered = true,
            Contact = new
            {
                Name = "John",
                Phone = "09119933311",
                Email = "user@dummy.com",
            },
            ModelId = _context.Models.OrderBy(i => i.Id).First().Id,
            FeatureIds = new[]
            {
                _context.VehicleFeatures.OrderBy(i => i.Id).First().Id
            }
        };

        var response = await _client.PostAsJsonAsync("/api/v1/vehicles", data);

        var vehicle = await response.Content.ReadFromJsonAsync<VehicleResource>();
        var exists = await _context.Vehicles.AnyAsync(
            v => v.Id == vehicle!.Id
                 && v.ContactEmail == data.Contact.Email
                 && v.ContactPhone == data.Contact.Phone);
        exists.ShouldBeTrue();
    }

    [Fact]
    public async Task GivenVehicleWhenCalledThenShouldResponseExpectedFields()
    {
        _context.Models.Add(ModelFactory.Create());
        _context.VehicleFeatures.Add(VehicleFeatureFactory.Create());
        await _context.SaveChangesAsync();
        var model = _context.Models
            .Include(m => m.Make)
            .OrderBy(i => i.Id).First();
        var data = new
        {
            Id = 1000,
            IsRegistered = true,
            Contact = new
            {
                Name = "John",
                Phone = "09119933311",
                Email = "user@dummy.com",
            },
            ModelId = model.Id,
            FeatureIds = new[]
            {
                _context.VehicleFeatures.OrderBy(i => i.Id).First().Id
            }
        };

        var response = await _client.PostAsJsonAsync("/api/v1/vehicles", data);

        var content = await response.Content.ReadFromJsonAsync<JsonElement>();
        content.GetProperty("id").GetInt32().ShouldBeGreaterThan(0);
        content.GetProperty("id").GetInt32().ShouldBeLessThan(1000);
        content.GetProperty("isRegistered").GetBoolean().ShouldBeEquivalentTo(data.IsRegistered);
        content.GetProperty("contact").GetProperty("name").GetString()
            .ShouldBeEquivalentTo(data.Contact.Name);
        content.GetProperty("contact").GetProperty("phone").GetString()
            .ShouldBeEquivalentTo(data.Contact.Phone);
        content.GetProperty("contact").GetProperty("email").ToString()
            .ShouldBeEquivalentTo(data.Contact.Email);
        content.GetProperty("make").GetProperty("id").GetInt32().ShouldBe(model.MakeId);
        content.GetProperty("make").GetProperty("name").GetString().ShouldBe(model.Make.Name);
        content.GetProperty("model").GetProperty("id").GetInt32().ShouldBe(model.Id);
        content.GetProperty("model").GetProperty("name").GetString().ShouldBe(model.Name);
    }

    [Fact]
    public async Task GivenCreateWhenServiceCalledThenVehicleShouldGetCreated()
    {
        _context.Models.Add(ModelFactory.Create());
        _context.VehicleFeatures.Add(VehicleFeatureFactory.Create());
        _context.SaveChanges();
        var data = new
        {
            Id = 1000,
            IsRegistered = true,
            Contact = new
            {
                Name = "John",
                Phone = "09119933311",
                Email = "user@dummy.com",
            },
            ModelId = _context.Models.OrderBy(i => i.Id).First().Id,
            FeatureIds = new[]
            {
                _context.VehicleFeatures.OrderBy(i => i.Id).First().Id
            }
        };

        var response = await _client.PostAsJsonAsync("/api/v1/vehicles", data);

        response.StatusCode.ShouldBe(HttpStatusCode.Created);
    }
}
