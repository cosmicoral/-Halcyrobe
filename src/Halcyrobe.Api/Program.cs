using System.Text.Json.Serialization;
using Halcyrobe.Api.Contracts;
using Halcyrobe.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IProductCatalog, InMemoryProductCatalog>();
builder.Services.AddSingleton<ITryOnJobService, InMemoryTryOnJobService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/health", () => Results.Ok(new { status = "ok" }))
    .WithName("HealthCheck")
    .WithTags("System");

app.MapGet(
        "/api/products",
        (string? brand, string? category, decimal? maxPrice, IProductCatalog catalog) =>
            Results.Ok(catalog.GetAll(brand, category, maxPrice)))
    .WithName("ListProducts")
    .WithTags("Products");

app.MapGet(
        "/api/products/{id:guid}",
        (Guid id, IProductCatalog catalog) =>
        {
            var product = catalog.GetById(id);
            return product is null ? Results.NotFound() : Results.Ok(product);
        })
    .WithName("GetProduct")
    .WithTags("Products");

app.MapPost(
        "/api/try-ons",
        (CreateTryOnRequest request, ITryOnJobService jobs) =>
        {
            var result = jobs.Create(request);

            if (result.Job is not null)
            {
                return Results.Created($"/api/try-ons/{result.Job.Id}", TryOnJobResponse.From(result.Job));
            }

            return result.ErrorCode switch
            {
                TryOnJobErrorCode.ProductNotFound => Results.NotFound(new { error = result.Error }),
                _ => Results.BadRequest(new { error = result.Error })
            };
        })
    .WithName("CreateTryOnJob")
    .WithTags("Try-ons");

app.MapGet(
        "/api/try-ons/{id:guid}",
        (Guid id, ITryOnJobService jobs) =>
        {
            var job = jobs.GetById(id);
            return job is null ? Results.NotFound() : Results.Ok(TryOnJobResponse.From(job));
        })
    .WithName("GetTryOnJob")
    .WithTags("Try-ons");

app.Run();

public partial class Program;

