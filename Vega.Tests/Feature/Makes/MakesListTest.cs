namespace Vega.Tests.Feature.Makes;

using System.Net;

using Newtonsoft.Json;

using Resources.V1;

using Shouldly;

using Support;

public class MakesListTest : IClassFixture<TestableWebApplicationFactory>
{
    private readonly HttpClient _client;

    public MakesListTest(TestableWebApplicationFactory factory) => _client = factory.CreateClient();

    [Fact]
    public async Task GivenGetWhenMakesCallServiceThenResponseShouldBeAsExpected()
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
    public async Task GivenGetWhenCallServiceThenShouldResponseOk()
    {
        var url = @"/api/v1/makes";

        var response = await _client.GetAsync(url);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
}
