namespace Vega.Tests.Feature.Vehicles;

using System.Net;
using System.Net.Http.Json;

using Factories;

using Persistence;

using Resources.V1;

using Shouldly;

using Support;

using Vehicle = Core.Domain.Vehicle;

public class ShowVehicleTest : IClassFixture<TestableWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly VegaDbContext _context;

    public ShowVehicleTest(TestableWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _context = factory.ResolveDbContext<VegaDbContext>();
    }

    [Fact]
    public async Task GivenVehicleIdWhenCalledThenReturnExpectingData()
    {
        var vehicle = await FactoryVehicle();
        var url = $"/api/v1/vehicles/{vehicle.Id}";

        var data = await _client.GetFromJsonAsync<VehicleResource>(url);

        data!.Id.ShouldBe(vehicle.Id);
        data.Model.Id.ShouldBe(vehicle.ModelId);
        data.VehicleFeatures.ShouldBeOfType<List<VehicleFeatureResource>>();
        data.Contact.Email.ShouldBe(vehicle.ContactEmail);
        data.Contact.Name.ShouldBe(vehicle.ContactName);
        data.Contact.Phone.ShouldBe(vehicle.ContactPhone);
    }

    [Fact]
    public async Task GivenVehicleIdWhenCalledThenShouldReturnOk()
    {
        var vehicle = await FactoryVehicle();
        var url = $"/api/v1/vehicles/{vehicle.Id}";

        var response = await _client.GetAsync(url);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    private async Task<Vehicle> FactoryVehicle()
    {
        var vehicle = VehicleFactory.Create();
        _context.Vehicles.Add(vehicle);
        await _context.SaveChangesAsync();

        return vehicle;
    }
}
