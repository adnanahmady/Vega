namespace Vega.Tests.Feature.Vehicles;

using System.Net;
using System.Net.Http.Json;

using Factories;

using Persistence;

using Resources.V1;

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
    public async Task GivenGetThenServiceCalledThenShouldReturnListOfVehicles()
    {
        _context.Vehicles.Add(VehicleFactory.Create());
        await _context.SaveChangesAsync();
        var response = await _client.GetAsync("/api/v1/vehicles");

        var data = await response.Content.ReadFromJsonAsync<List<VehicleResource>>();

        var item = data!.First();
        item.Id.ShouldBeOfType<int>();
        item.IsRegistered.ShouldBeOfType<bool>();
        item.Contact.Name.ShouldBeOfType<string>().ShouldNotBeNullOrWhiteSpace();
        item.Contact.Phone.ShouldBeOfType<string>().ShouldNotBeNullOrWhiteSpace();
        item.Contact.Email.ShouldBeOfType<string>().ShouldNotBeNullOrWhiteSpace();
        var make = item.Make.ShouldBeOfType<KeyValuePairResource>();
        make.Id.ShouldBeOfType<int>();
        var model = item.Model.ShouldBeOfType<KeyValuePairResource>();
        model.Id.ShouldBeOfType<int>();
        model.Name.ShouldBeOfType<string>();
        var vehicleFeature = item.VehicleFeatures
            .ShouldBeOfType<List<KeyValuePairResource>>();
        vehicleFeature.First().Id.ShouldBeOfType<int>();
        vehicleFeature.First().Name.ShouldBeOfType<string>();
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
