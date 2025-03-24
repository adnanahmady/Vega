using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

using Shouldly;

using Vega.Persistence;
using Vega.Tests.Factories;
using Vega.Tests.Support;

namespace Vega.Tests.Feature.Vehicles;

public class VehiclePhotosListTest : IClassFixture<TestableWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly VegaDbContext _context;

    public VehiclePhotosListTest(TestableWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _context = factory.ResolveDbContext<VegaDbContext>();
    }

    [Fact]
    public async Task GivenVehiclePhotoWhenInvalidVehicleIdPassedThenShouldReturnEmptyList()
    {
        var url = @"/api/v1/vehicles/99999/photos";

        var response = await _client.GetFromJsonAsync<JsonElement>(url);

        response.EnumerateArray().Count().ShouldBe(0);
    }

    [Fact]
    public async Task GivenVehiclePhotoWhenVehicleIdPassedThenShouldReturnItPhotos()
    {
        var photo = VehiclePhotoFactory.Create();
        _context.Add(photo);
        await _context.SaveChangesAsync();
        var url = $"/api/v1/vehicles/{photo.VehicleId}/photos";

        var response = await _client.GetFromJsonAsync<JsonElement>(url);

        var item = response.EnumerateArray().First()!;
        item.GetProperty("id").GetInt64().ShouldBe(photo.Id);
        item.GetProperty("name").GetString().ShouldBe(photo.Name);
    }

    [Fact]
    public async Task GivenVehiclePhotoWhenVehicleIdPassedThenStatusShouldBeOk()
    {
        var photo = VehiclePhotoFactory.Create();
        await _context.SaveChangesAsync();
        var url = $"/api/v1/vehicles/{photo.VehicleId}/photos";

        var response = await _client.GetAsync(url);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
}
