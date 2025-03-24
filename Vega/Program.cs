using Microsoft.EntityFrameworkCore;

using Vega.Core;
using Vega.Core.Domain;
using Vega.Core.QueryFilters;
using Vega.Persistence;
using Vega.Persistence.QueryFilters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<VegaDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("Default"),
    opt => opt.EnableRetryOnFailure()
));
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.Configure<PhotoSettings>(
    builder.Configuration.GetSection("PhotoSettings"));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IVehiclesListParamProcessor, VehiclesListParamProcessor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if (!app.Environment.IsEnvironment("Testing"))
{
    app.UseCors("AllowAll");
    app.UseHttpsRedirection();
}
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();

public partial class Program { }
