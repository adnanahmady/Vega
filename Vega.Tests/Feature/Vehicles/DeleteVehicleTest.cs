namespace Vega.Tests.Feature.Vehicles;

using System.Net;

using Domain;

using Factories;

using Microsoft.EntityFrameworkCore;

using Shouldly;

using Support;

using Vehicle = Models.Vehicle;

public class DeleteVehicleTest : IClassFixture<TestableWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly VegaDbContext _context;

    public DeleteVehicleTest(TestableWebApplicationFactory factory)
    {
        this._client = factory.CreateClient();
        this._context = factory.ResolveDbContext<VegaDbContext>();
    }

    [Fact]
    public async Task GivenWrongVehicleIdWhenCalledThenShouldThrowNotFoundError()
    {
        var url = $"/api/v1/vehicles/10";

        var data = await this._client.DeleteAsync(url);

        data.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GivenVehicleIdWhenCalledThenDeleteVehicle()
    {
        var vehicle = await this.FactoryVehicle();
        var url = $"/api/v1/vehicles/{vehicle.Id}";

        await this._client.DeleteAsync(url);

        var count = await this._context.Vehicles
            .Where(v => v.Id == vehicle.Id).CountAsync();
        count.ShouldBe(0);
    }

    [Fact]
    public async Task GivenVehicleIdWhenCalledThenShouldReturnNoContent()
    {
        var vehicle = await this.FactoryVehicle();
        var url = $"/api/v1/vehicles/{vehicle.Id}";

        var response = await this._client.DeleteAsync(url);

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    private async Task<Vehicle> FactoryVehicle()
    {
        var vehicle = VehicleFactory.Create();
        this._context.Vehicles.Add(vehicle);
        await this._context.SaveChangesAsync();

        return vehicle;
    }
}
