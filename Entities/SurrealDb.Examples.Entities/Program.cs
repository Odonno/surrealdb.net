using Scalar.AspNetCore;
using SurrealDb.Examples.Entities.Models;
using SurrealDb.Net;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

services
    .AddEndpointsApiExplorer()
    .AddOpenApi(options =>
    {
        options.AddDocumentTransformer(
            (document, _, _) =>
            {
                document.Info.Title = "Entities Examples API";
                document.Info.Version = "v1";

                return Task.CompletedTask;
            }
        );
    });

services.AddSurreal(configuration.GetConnectionString("SurrealDB")!);

var app = builder.Build();

app.UseHttpsRedirection();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // 💡 Enable OpenAPI document generation (e.g. "/openapi/v1.json")
    app.MapOpenApi();

    // 💡 Display OpenAPI User Interfaces (Swagger UI, Scalar)
    app.UseSwaggerUI(options =>
    {
        options.ConfigObject.Urls =
        [
            new UrlDescriptor { Name = "Entities Examples API v1", Url = "/openapi/v1.json" },
        ];
    });
    app.MapScalarApiReference();
}

var api = app.MapGroup("/api");
api.MapGet("/weatherForecast", (ISurrealDbClient client) => client.WeatherForecasts);
api.MapGet("/todo", (ISurrealDbClient client) => client.Todos);

await InitializeDbAsync();

app.Run();

async Task InitializeDbAsync()
{
    const int initialCount = 5;
    var weatherForecasts = new WeatherForecastFaker().Generate(initialCount);
    var surrealDbClient = new SurrealDbClient(
        SurrealDbOptions
            .Create()
            .FromConnectionString(configuration.GetConnectionString("SurrealDB")!)
            .Build()
    );

    var tasks = weatherForecasts.Select(weatherForecast =>
        surrealDbClient.Create(WeatherForecast.Table, weatherForecast)
    );

    await Task.WhenAll(tasks);
}

// 💡 Will generate extension properties
[EntitiesGeneratorRecordsFromNamespace("SurrealDb.Examples.Entities.Models")]
public class MyEntitiesGenerator;
