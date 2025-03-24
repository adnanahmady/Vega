using System.Net;
using System.Net.Http.Headers;
using System.Text;

using Microsoft.EntityFrameworkCore;

using Shouldly;

using Vega.Persistence;
using Vega.Tests.Factories;
using Vega.Tests.Support;

using MultipartFormDataContent = System.Net.Http.MultipartFormDataContent;
using StreamContent = System.Net.Http.StreamContent;

namespace Vega.Tests.Feature.Vehicles;

public class UploadVehiclePhotoTest : IClassFixture<TestableWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly VegaDbContext _context;

    public UploadVehiclePhotoTest(TestableWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _context = factory.ResolveDbContext<VegaDbContext>();
    }

    [Fact]
    public async Task GivenPhotoWhenStoredThenShouldSaveToDatabase()
    {
        // Arrange
        var vehicle = VehicleFactory.Create();
        _context.Vehicles.Add(vehicle);
        await _context.SaveChangesAsync();

        var content = Encoding.UTF8.GetBytes("Fake image");
        var streamContent = new StreamContent(new MemoryStream(content));
        streamContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

        var formData = new MultipartFormDataContent
        {
            { streamContent, "file", "test.jpeg" }
        };

        var url = $"/api/v1/vehicles/{vehicle.Id}/photos";

        // Act
        var response = await _client.PostAsync(url, formData);

        // Assert
        var count = await _context
            .VehiclePhotos
            .CountAsync(p => p.Vehicle.Id == vehicle.Id);
        count.ShouldBeEquivalentTo(1);
    }

    [Fact]
    public async Task GivenPhotoWhenCalledThenShouldReturnOk()
    {
        // Arrange
        var vehicle = VehicleFactory.Create();
        _context.Vehicles.Add(vehicle);
        await _context.SaveChangesAsync();

        var content = Encoding.UTF8.GetBytes("Fake image content");
        var streamContent = new StreamContent(new MemoryStream(content));
        streamContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

        var formData = new MultipartFormDataContent
        {
            { streamContent, "file", "test.jpg" }
        };

        var url = $"/api/v1/vehicles/{vehicle.Id}/photos";

        // Act
        var response = await _client.PostAsync(url, formData);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
}
