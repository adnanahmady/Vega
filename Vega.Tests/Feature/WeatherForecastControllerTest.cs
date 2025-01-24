using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Shouldly;
using Xunit.Abstractions;

namespace Vega.Tests.Feature;

public class WeatherForecastControllerTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _output;

    public WeatherForecastControllerTest(
        WebApplicationFactory<Program> factory,
        ITestOutputHelper output)
    {
        _client = factory.CreateClient();
        _output = output;
    }

    [Fact]
    public async Task Get_CallService_ShouldReturnItemsAsExpected()
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
    public async Task Get_CallService_ShouldResponseOk()
    {
        var url = "/WeatherForecast";

        var response = await _client.GetAsync(url);
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
}