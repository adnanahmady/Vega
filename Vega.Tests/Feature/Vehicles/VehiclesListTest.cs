using System.Text.Json;

using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;

using Vega.Core.Domain;

namespace Vega.Tests.Feature.Vehicles;

using System.Net;
using System.Net.Http.Json;

using Factories;

using Persistence;

using Shouldly;

using Support;

public class VehiclesListTest : IClassFixture<TestableWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly VegaDbContext _context;

    public VehiclesListTest(TestableWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _context = factory.ResolveDbContext<VegaDbContext>();
    }

    public static IEnumerable<object[]> MemberDataForFilterTest()
    {
        yield return new object[]
        {
            new Func<Make, Make, Make, Model, Model, Dictionary<string, string?>>(
            (make1, make2, make3, model1, model2) => new Dictionary<string, string?>
            {
                { "makeId", $"{make1.Id}" }
            }),
            1,
            "given 1st make when called then should return 1 vehicle"
        };

        yield return new object[]
        {
            new Func<Make, Make, Make, Model, Model, Dictionary<string, string?>>(
            (make1, make2, make3, model1, model2) => new Dictionary<string, string?>
            {
                { "makeId", $"{make2.Id}" }
            }),
            2,
            "given 2nd make when called then should return 2 vehicles"
        };

        yield return new object[]
        {
            new Func<Make, Make, Make, Model, Model, Dictionary<string, string?>>(
            (make1, make2, make3, model1, model2) => new Dictionary<string, string?>
            {
                { "makeId", $"{make3.Id}" }
            }),
            0,
            "given 3rd make when called then should return empty list"
        };

        yield return new object[]
        {
            new Func<Make, Make, Make, Model, Model, Dictionary<string, string?>>(
            (make1, make2, make3, model1, model2) => new Dictionary<string, string?>
            {
                { "makeId", "1000" }
            }),
            0,
            "given not existing make when called then should return empty list"
        };

        yield return new object[]
        {
            new Func<Make, Make, Make, Model, Model, Dictionary<string, string?>>(
            (make1, make2, make3, model1, model2) => new Dictionary<string, string?>
            {
                { "makeId", null }
            }),
            3,
            "given make filter when value is null then should return all 3 vehicles"
        };

        yield return new object[]
        {
            new Func<Make, Make, Make, Model, Model, Dictionary<string, string?>>(
            (make1, make2, make3, model1, model2) => new Dictionary<string, string?>
            {
                { "makeId", "" }
            }),
            3,
            "given empty make filter when value is null then should return all 3 vehicles"
        };

        yield return new object[]
        {
            new Func<Make, Make, Make, Model, Model, Dictionary<string, string?>>(
            (make1, make2, make3, model1, model2) => new Dictionary<string, string?>
            {
                { "modelId", $"{model1.Id}" }
            }),
            1,
            "given 1st model when called then should return 1 vehicle"
        };

        yield return new object[]
        {
            new Func<Make, Make, Make, Model, Model, Dictionary<string, string?>>(
            (make1, make2, make3, model1, model2) => new Dictionary<string, string?>
            {
                { "modelId", $"{model2.Id}" }
            }),
            2,
            "given 2nd model when called then should return 2 vehicles"
        };

        yield return new object[]
        {
            new Func<Make, Make, Make, Model, Model, Dictionary<string, string?>>(
            (make1, make2, make3, model1, model2) => new Dictionary<string, string?>
            {
                { "modelId", "3000" }
            }),
            0,
            "given not existing model when called then should return empty list"
        };

        yield return new object[]
        {
            new Func<Make, Make, Make, Model, Model, Dictionary<string, string?>>(
            (make1, make2, make3, model1, model2) => new Dictionary<string, string?>
            {
                { "modelId", null }
            }),
            3,
            "given model filter when value is null then should return all 3 vehicles"
        };
    }

    [Theory]
    [MemberData(nameof(MemberDataForFilterTest))]
    public async Task GivenFilterWhenCalledThenShouldReturnExpectedData(
        Func<Make, Make, Make, Model, Model, Dictionary<string, string?>> queryString,
        int expectedCount,
        string scenario)
    {
        await _context.Vehicles.ExecuteDeleteAsync();
        await _context.Models.ExecuteDeleteAsync();
        await _context.Makes.ExecuteDeleteAsync();
        var make1 = MakeFactory.Create(name: (_, _) => "A Make");
        var model1 = ModelFactory.Create((_, _) => "A Model", (_, _) => make1);
        _context.Vehicles.Add(VehicleFactory.Create(model: (_, _) => model1));
        var make2 = MakeFactory.Create(name: (_, _) => "B Make");
        var model2 = ModelFactory.Create((_, _) => "B Model", (_, _) => make2);
        var make3 = MakeFactory.Create(name: (_, _) => "C Make");
        _context.Makes.Add(make3);
        _context.Vehicles.Add(VehicleFactory.Create(model: (_, _) => model2));
        _context.Vehicles.Add(VehicleFactory.Create(model: (_, _) => model2));
        await _context.SaveChangesAsync();
        var url = QueryHelpers.AddQueryString(
            "/api/v1/vehicles",
            queryString(make1, make2, make3, model1, model2)
        );

        var response = await _client.GetAsync(url);

        var content = await response.Content.ReadFromJsonAsync<JsonElement>();
        content.GetProperty("data").GetArrayLength().ShouldBe(expectedCount);
    }

    public static IEnumerable<object[]> MemberDataForSortingTest()
    {
        yield return new object[]
        {
            new Dictionary<string, string?>()
                    {
                        { "sortBy", "make" },
                        { "sortDirection", "desc" }
                    },
            new Action<JsonElement>(data =>
                data.GetProperty("make").GetProperty("name").GetString().ShouldBe("C Make")
            ),
            "given make when its desc then `c make` should be the first one"
        };

        yield return new object[]
        {
            new Dictionary<string, string?>()
                    {
                        { "sortBy", "make" },
                        { "sortDirection", "asc" }
                    },
            new Action<JsonElement>(data =>
                data.GetProperty("make").GetProperty("name").GetString().ShouldBe("A Make")
            ),
            "given make when its asc then `a make` should be the first one"
        };

        yield return new object[]
        {
            new Dictionary<string, string?>()
                    {
                        { "sortBy", "model" },
                        { "sortDirection", "desc" }
                    },
            new Action<JsonElement>(data =>
                data.GetProperty("model").GetProperty("name").GetString().ShouldBe("C Model")
            ),
            "given model when its desc then `c model` should be the first one"
        };

        yield return new object[]
        {
            new Dictionary<string, string?>()
                    {
                        { "sortBy", "model" },
                        { "sortDirection", "asc" }
                    },
            new Action<JsonElement>(data =>
                data.GetProperty("model").GetProperty("name").GetString().ShouldBe("A Model")
            ),
            "given model when its desc then `a model` should be the first one"
        };

        yield return new object[]
        {
            new Dictionary<string, string?>()
                    {
                        { "sortBy", "contact-name" },
                        { "sortDirection", "desc" }
                    },
            new Action<JsonElement>(data =>
                data.GetProperty("contact").GetProperty("name").GetString().ShouldBe("C User")
            ),
            "given contact name when its desc then `c user` should be the first one"
        };

        yield return new object[]
        {
            new Dictionary<string, string?>()
                    {
                        { "sortBy", "contact-name" },
                        { "sortDirection", "asc" }
                    },
            new Action<JsonElement>(data =>
                data.GetProperty("contact").GetProperty("name").GetString().ShouldBe("A User")
            ),
            "given contact name when its desc then `a user` should be the first one"
        };
    }

    [Theory]
    [MemberData(nameof(MemberDataForSortingTest))]
    public async Task GivenSortWhenCalledThenShouldReturnWithExpectedSort(
        Dictionary<string, string?> queryString,
        Action<JsonElement> assertion,
        // the scenario parameter is useful to determine the failing iteration when test fails
        string scenario
    )
    {
        var items = new[]
        {
            new[] { "A User", "C Model", "B Make" },
            new[] { "C User", "B Model", "A Make" },
            new[] { "B User", "A Model", "C Make" },
        };
        foreach (var item in items)
        {
            _context.Vehicles.Add(VehicleFactory.Create(
                contactName: (_, _) => item[0],
                model: (_, _) => ModelFactory.Create(
                    name: (_, _) => item[1],
                    (_, _) => MakeFactory.Create(name: (_, _) => item[2])
                )));
        }
        await _context.SaveChangesAsync();
        var url = QueryHelpers.AddQueryString(uri: "/api/v1/vehicles",
            queryString: queryString);

        var response = await _client.GetAsync(url);

        var content = await response.Content.ReadFromJsonAsync<JsonElement>();
        var data = content.GetProperty("data").EnumerateArray().First();
        assertion(data);
    }

    [Fact]
    public async Task GivenListWhenResponseReturnedThenShouldBePaginated()
    {
        await _context.Vehicles.ExecuteDeleteAsync();
        _context.Vehicles.Add(VehicleFactory.Create());
        await _context.SaveChangesAsync();
        var queryString = new Dictionary<string, string?>()
        {
            { "pageNumber", "1" },
            { "pageSize", "10" },
        };
        var url = QueryHelpers.AddQueryString(uri: "/api/v1/vehicles",
            queryString: queryString);
        var response = await _client.GetAsync(url);

        var content = await response.Content.ReadFromJsonAsync<JsonElement>();

        var meta = content.GetProperty("meta");
        var pagination = meta.GetProperty("pagination");
        pagination.GetProperty("currentPage").GetInt32().ShouldBe(1);
        pagination.GetProperty("totalRecords").GetInt32().ShouldBe(1);
        pagination.GetProperty("from").GetInt32().ShouldBe(0);
        pagination.GetProperty("to").GetInt32().ShouldBe(10);
        pagination.GetProperty("pageSize").GetInt32().ShouldBe(10);
    }

    [Fact]
    public async Task GivenGetThenServiceCalledThenShouldReturnListOfVehicles()
    {
        _context.Vehicles.Add(VehicleFactory.Create());
        await _context.SaveChangesAsync();
        var response = await _client.GetAsync("/api/v1/vehicles");

        var content = await response.Content.ReadFromJsonAsync<JsonElement>();

        var data = content.GetProperty("data").EnumerateArray();
        var item = data.First();
        item.GetProperty("id").GetInt32().ShouldBeOfType<int>();
        item.GetProperty("isRegistered").GetBoolean().ShouldBeOfType<bool>();
        var contact = item.GetProperty("contact");
        contact.GetProperty("name").GetString()
            .ShouldBeOfType<string>().ShouldNotBeNullOrWhiteSpace();
        contact.GetProperty("phone").GetString()
            .ShouldBeOfType<string>().ShouldNotBeNullOrWhiteSpace();
        contact.GetProperty("email").GetString()
            .ShouldBeOfType<string>().ShouldNotBeNullOrWhiteSpace();
        var make = item.GetProperty("make");
        make.GetProperty("id").GetInt32().ShouldBeOfType<int>();
        make.GetProperty("name").GetString().ShouldBeOfType<string>();
        var model = item.GetProperty("model");
        model.GetProperty("id").GetInt32().ShouldBeOfType<int>();
        model.GetProperty("name").GetString().ShouldBeOfType<string>();
        var features = item.GetProperty("vehicleFeatures").EnumerateArray();
        features.First().GetProperty("id").GetInt32().ShouldBeOfType<int>();
        features.First().GetProperty("name").GetString().ShouldBeOfType<string>();
    }

    [Fact]
    public async Task GivenGetWhenServiceCalledThenShouldReturnOkStatusCode()
    {
        _context.Vehicles.Add(VehicleFactory.Create());
        _context.SaveChanges();

        var response = await _client.GetAsync("/api/v1/vehicles");

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
}
