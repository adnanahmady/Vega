namespace Vega.Tests.Feature.VehicleFeatures;

using System.Net;

using Newtonsoft.Json;

using Shouldly;

using Support;

using Vega.Resources.V1;

public class VehicleFeaturesListTest : IClassFixture<TestableWebApplicationFactory>
{
    private readonly HttpClient _client;

    public VehicleFeaturesListTest(
        TestableWebApplicationFactory factory) => _client = factory.CreateClient();

    [Fact]
    public async Task GivenGetFeaturesWhenCallServiceThenShouldBeAListOfVehicleFeatures()
    {
        var url = @"api/v1/features";
        var response = await _client.GetAsync(url);

        var content = await response.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<List<VehicleFeatureResource>>(content);
        var item = data.First();

        item.Id.ShouldBeOfType<int>();
        item.Name.ShouldBeOfType<string>();
    }

    [Fact]
    public async Task GivenGetFeaturesWhenCallServiceThenShouldResponseOk()
    {
        var url = @"api/v1/features";

        var response = await _client.GetAsync(url);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
}
