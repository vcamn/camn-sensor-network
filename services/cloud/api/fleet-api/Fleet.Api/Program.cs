using Fleet.Api.Contracts;
using Fleet.Api.Services;
using Fleet.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<FleetDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("FleetDb"),
        npgsqlOptions =>
        {
            npgsqlOptions.MigrationsHistoryTable(
                "__EFMigrationsHistory", 
                "admin");
        }   
    )
    .UseSnakeCaseNamingConvention();
});

builder.Services.AddScoped<ISiteStatusService, SiteStatusService>();
builder.Services.AddScoped<IStationStatusService, StationStatusService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Handle circular references gracefully by ignoring them during serialization
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
