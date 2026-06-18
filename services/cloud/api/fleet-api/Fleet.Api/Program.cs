using Asp.Versioning;
using Fleet.Api.Contracts;
using Fleet.Api.Services;
using Fleet.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

/*** Add services to the container. ***/

// Configure Entity Framework Core with PostgreSQL and specify the migrations history table
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

// Register application services for dependency injection
builder.Services.AddScoped<ISiteStatusService, SiteStatusService>();
builder.Services.AddScoped<IStationStatusService, StationStatusService>();
builder.Services.AddScoped<ISensorStatusService, SensorStatusService>();
builder.Services.AddScoped<IDeviceStatusService, DeviceStatusService>();
builder.Services.AddScoped<ISensorTypeService, SensorTypeService>();

// Configure controllers and JSON options to handle circular references gracefully
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Handle circular references gracefully by ignoring them during serialization
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// Configure API versioning to support multiple versions of the API
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
})
.AddMvc()
.AddOpenApi();

// Build the application
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Enable OpenAPI/Swagger documentation with a document per API version
    app.MapOpenApi().WithDocumentPerVersion();

    // Configure Swagger UI to display the API documentation for each version
    app.UseSwaggerUI(options =>
    {
        foreach (var description in app.DescribeApiVersions())
        {
            options.SwaggerEndpoint(
                $"../openapi/{description.GroupName}.json",
                description.GroupName.ToUpperInvariant()
            );
        }
    });

    // Configure Scalar API reference documentation for each API version
    app.MapScalarApiReference(options =>
    {
        var descriptions = app.DescribeApiVersions();
        for (var i = 0; i < descriptions.Count; i++)
        {
            var description = descriptions[i];
            //var isDefault = i == descriptions.Count - 1;

            options.AddDocument(description.GroupName, description.GroupName);
        }
    });
}

// Enable HTTPS redirection and authorization middleware
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
