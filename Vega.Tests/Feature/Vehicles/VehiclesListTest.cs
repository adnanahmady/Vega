using System.Text.Json;

using Bogus.DataSets;

using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;

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

    [Fact]
    public async Task GivenSortWhenCalledThenShouldReturnWithExpectedSort()
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
        var queryString = new Dictionary<string, string?>()
        {
            { "sortBy", "make" },
            { "sortDirection", "desc" }
        };
        var url = QueryHelpers.AddQueryString(uri: "/api/v1/vehicles",
            queryString: queryString);

        var response = await _client.GetAsync(url);

        var content = await response.Content.ReadFromJsonAsync<JsonElement>();
        var data = content.GetProperty("data").EnumerateArray().First();
        data.GetProperty("make").GetProperty("name").GetString().ShouldBe("C Make");
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
