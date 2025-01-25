using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Shouldly;
using Vega.Dtos.V1;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Vega.Tests.Feature.Makes;

public class MakesListTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _output;

    public MakesListTest(
        WebApplicationFactory<Program> factory,
        ITestOutputHelper output)
    {
        _client = factory.CreateClient();
        _output = output;
    }

    [Fact]
    public async Task GetMakes_CallService_ResponseShouldBeAsExpected()
    {
        var url = @"/api/v1/makes";
        var response = await _client.GetAsync(url);

        string content = await response.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<List<MakeDto>>(content);
        var dto = data.First();

        dto.Id.ShouldBeOfType<int>();
        dto.Name.ShouldBeOfType<string>();
        dto.Models.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task GetMakes_CallService_ShouldResponseOk()
    {
        var url = @"/api/v1/makes";

        var response = await _client.GetAsync(url);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
}
