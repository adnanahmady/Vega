using System.Net;
using Newtonsoft.Json;
using Shouldly;

namespace Vega.Tests.Feature;

using Support;

public class WeatherForecastControllerTest : IClassFixture<TestableWebApplicationFactory>
{
    private readonly HttpClient _client;

    public WeatherForecastControllerTest(
        TestableWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GivenGetWhenCallServiceThenShouldReturnItemsAsExpected()
    {
        var url = "/WeatherForecast";
        var response = await _client.GetAsync(url);

        var content = await response.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<List<WeatherForecast>>(content);

        data.ShouldNotBeEmpty();
        data[0].Date.ShouldBeOfType<DateOnly>();
        data[0].Summary.ShouldNotBeNullOrEmpty();
        data[0].TemperatureC.ShouldBeOfType<int>();
        data[0].TemperatureF.ShouldBeOfType<int>();
    }

    [Fact]
    public async Task GivenGetWhenCallServiceThenShouldResponseOk()
    {
        var url = "/WeatherForecast";

        var response = await _client.GetAsync(url);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
}
