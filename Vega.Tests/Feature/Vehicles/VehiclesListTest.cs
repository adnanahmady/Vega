namespace Vega.Tests.Feature.Vehicles;

using System.Net;
using System.Net.Http.Json;
using Domain;
using Factories;
using Newtonsoft.Json;
using Resources.V1;
using Shouldly;
using Support;

public class VehiclesListTest : IClassFixture<TestableWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly VegaDbContext _context;

    public VehiclesListTest(TestableWebApplicationFactory factory)
    {
        this._client = factory.CreateClient();
        this._context = factory.ResolveDbContext<VegaDbContext>();
    }

    [Fact]
    public async Task GivenGetThenServiceCalledThenShouldReturnListOfVehicles()
    {
        this._context.Vehicles.Add(VehicleFactory.Create());
        await this._context.SaveChangesAsync();
        var response = await this._client.GetAsync("/api/v1/vehicles");

        var data = await response.Content.ReadFromJsonAsync<List<VehicleResource>>();

        var item = data.First();
        item.Id.ShouldBeOfType<int>();
        item.IsRegistered.ShouldBeOfType<bool>();
        item.ContactName.ShouldBeOfType<string>().ShouldNotBeNullOrWhiteSpace();
        item.ContactPhone.ShouldBeOfType<string>().ShouldNotBeNullOrWhiteSpace();
        item.ContactEmail.ShouldBeOfType<string>().ShouldNotBeNullOrWhiteSpace();
        var model = item.Model.ShouldBeOfType<ModelResource>();
        model.Id.ShouldBeOfType<int>();
        model.Name.ShouldBeOfType<string>();
        var vehicleFeature = item.VehicleFeatures
            .ShouldBeOfType<List<VehicleFeatureResource>>();
        vehicleFeature.First().Id.ShouldBeOfType<int>();
        vehicleFeature.First().Name.ShouldBeOfType<string>();
    }

    [Fact]
    public async Task GivenGetWhenServiceCalledThenShouldReturnOkStatusCode()
    {
        this._context.Vehicles.Add(VehicleFactory.Create());
        this._context.SaveChanges();

        var response = await this._client.GetAsync("/api/v1/vehicles");

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

}
