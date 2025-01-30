namespace Vega.Tests.Feature.Makes;

using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Shouldly;
using Resources.V1;
using Xunit.Abstractions;

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

        var content = await response.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<List<MakeResource>>(content);
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
