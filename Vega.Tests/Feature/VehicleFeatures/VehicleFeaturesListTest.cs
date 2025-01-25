using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Shouldly;
using Vega.Dtos.V1;
using Xunit.Abstractions;

namespace Vega.Tests.Feature.VehicleFeatures;

public class VehicleFeaturesListTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _output;

    public VehicleFeaturesListTest(
        WebApplicationFactory<Program> factory,
        ITestOutputHelper output)
    {
        _client = factory.CreateClient();
        _output = output;
    }

    [Fact]
    public async Task GetFeatures_CallService_ShouldBeAListOfVehicleFeatures()
    {
        var url = @"api/v1/features";
        var response = await _client.GetAsync(url);

        var content = await response.Content.ReadAsStringAsync();
        _output.WriteLine("Content: >>>>>>>>>>>>>>>>>> : " + content);
        var data = JsonConvert.DeserializeObject<List<VehicleFeatureDto>>(content);
        var item = data.First();

        item.Id.ShouldBeOfType<int>();
        item.Name.ShouldBeOfType<string>();
    }

    [Fact]
    public async Task GetFeatures_CallService_ShouldResponseOk()
    {
        var url = @"api/v1/features";

        var response = await _client.GetAsync(url);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
}
