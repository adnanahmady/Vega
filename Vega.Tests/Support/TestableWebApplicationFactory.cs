using Microsoft.AspNetCore.Authentication;

namespace Vega.Tests.Support;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Persistence;

public class TestableWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        LoadTestSettingFile(builder);

        builder.ConfigureServices((c, services) =>
        {
            RemoveExistingDbContext(services);

            AddDbContextWithTestConnectionString(c, services);

            ApplyMigrationsBeforeTesting(services);
        });
    }

    public T ResolveDbContext<T>() where T : class => this
        .Services
        .CreateScope()
        .ServiceProvider
        .GetRequiredService<T>();

    private void LoadTestSettingFile(IWebHostBuilder builder) => builder
        .ConfigureAppConfiguration((c, config) =>
        {
            config.AddJsonFile("appsettings.Testing.json", optional: false, reloadOnChange: true);
        });

    private void RemoveExistingDbContext(IServiceCollection services)
    {
        var descriptor = services.SingleOrDefault(
            d => d.ServiceType == typeof(DbContextOptions<VegaDbContext>));
        if (descriptor != null)
        {
            services.Remove(descriptor);
        }
    }

    private void AddDbContextWithTestConnectionString(
        WebHostBuilderContext context,
        IServiceCollection services)
    {
        var connectionString = context.Configuration.GetConnectionString("Test");
        services.AddDbContext<VegaDbContext>(
            options => options.UseSqlServer(connectionString)
        );
    }

    private void ApplyMigrationsBeforeTesting(IServiceCollection services)
    {
        using var scope = services.BuildServiceProvider().CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<VegaDbContext>();
        context.Database.EnsureDeleted();
        context.Database.Migrate();
    }

    public WebApplicationFactory<Program> Authenticate() =>
        WithWebHostBuilder(builder => builder
            .ConfigureServices(services => services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "Test";
                    options.DefaultChallengeScheme = "Test";
                })
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                    "Test", o => { })));
}
