using Vega.Core.Domain;

namespace Vega.Tests.Feature.Vehicles;

using System.Net;

using Factories;

using Microsoft.EntityFrameworkCore;

using Persistence;

using Shouldly;

using Support;

public class DeleteVehicleTest : IClassFixture<TestableWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly VegaDbContext _context;

    public DeleteVehicleTest(TestableWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _context = factory.ResolveDbContext<VegaDbContext>();
    }

    [Fact]
    public async Task GivenWrongVehicleIdWhenCalledThenShouldThrowNotFoundError()
    {
        var url = $"/api/v1/vehicles/10";

        var data = await _client.DeleteAsync(url);

        data.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GivenVehicleIdWhenCalledThenDeleteVehicle()
    {
        var vehicle = await FactoryVehicle();
        var url = $"/api/v1/vehicles/{vehicle.Id}";

        await _client.DeleteAsync(url);

        var count = await _context.Vehicles
            .Where(v => v.Id == vehicle.Id).CountAsync();
        count.ShouldBe(0);
    }

    [Fact]
    public async Task GivenVehicleIdWhenCalledThenShouldReturnNoContent()
    {
        var vehicle = await FactoryVehicle();
        var url = $"/api/v1/vehicles/{vehicle.Id}";

        var response = await _client.DeleteAsync(url);

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    private async Task<Vehicle> FactoryVehicle()
    {
        var vehicle = VehicleFactory.Create();
        _context.Vehicles.Add(vehicle);
        await _context.SaveChangesAsync();

        return vehicle;
    }
}
