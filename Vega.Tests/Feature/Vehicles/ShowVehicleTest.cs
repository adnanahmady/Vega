namespace Vega.Tests.Feature.Vehicles;

using System;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Bogus.DataSets;
using Domain;
using Factories;
using Newtonsoft.Json;
using Resources.V1;
using Shouldly;
using Support;
using Vehicle = Models.Vehicle;

public class ShowVehicleTest : IClassFixture<TestableWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly VegaDbContext _context;

    public ShowVehicleTest(TestableWebApplicationFactory factory)
    {
        this._client = factory.CreateClient();
        this._context = factory.ResolveDbContext<VegaDbContext>();
    }

    [Fact]
    public async Task GivenVehicleIdWhenCalledThenReturnExpectingData()
    {
        var vehicle = await this.FactoryVehicle();
        var url = $"/api/v1/vehicles/{vehicle.Id}";

        var data = await this._client.GetFromJsonAsync<VehicleResource>(url);

        data.Id.ShouldBe(vehicle.Id);
        data.Model.Id.ShouldBe(vehicle.ModelId);
        data.VehicleFeature.ShouldBeOfType<VehicleFeatureResource>();
        data.ContactEmail.ShouldBe(vehicle.ContactEmail);
        data.ContactName.ShouldBe(vehicle.ContactName);
        data.ContactPhone.ShouldBe(vehicle.ContactPhone);
    }

    [Fact]
    public async Task GivenVehicleIdWhenCalledThenShouldReturnOk()
    {
        var vehicle = await this.FactoryVehicle();
        var url = $"/api/v1/vehicles/{vehicle.Id}";

        var response = await this._client.GetAsync(url);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    private async Task<Vehicle> FactoryVehicle()
    {
        var vehicle = VehicleFactory.Create();
        this._context.Vehicles.Add(vehicle);
        await this._context.SaveChangesAsync();

        return vehicle;
    }
}
