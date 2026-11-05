# Halcyrobe

Halcyrobe is a small ASP.NET Core API for fashion discovery and virtual try-on job orchestration. This first version is deliberately compact: it is intended to be understandable in a weekend while still demonstrating production-style C# structure, dependency injection, validation, tests, and API documentation.

The API does not call a real image-generation model yet. It provides the product catalogue and job boundary where a real provider can be connected later.

## What works

- Browse a seeded fashion catalogue.
- Filter products by brand, category, and maximum price.
- Fetch one product by ID.
- Create a virtual try-on job for a product and person-image URL.
- Retrieve a previously created job.
- Reject unknown products and invalid image URLs with clear errors.
- Run unit tests for filtering, validation, and job creation.
- Explore the endpoints through Swagger in development.

## Project structure

```text
Halcyrobe.sln
src/Halcyrobe.Api/
  Contracts/       HTTP request and response shapes
  Models/          Core product and try-on data
  Services/        Interfaces and in-memory implementations
  Program.cs       Dependency injection and HTTP endpoints
tests/Halcyrobe.Api.Tests/
  ProductCatalogTests.cs
  TryOnJobServiceTests.cs
```

The separation is intentional:

- `Models` describe the data the application works with.
- `Contracts` describe what clients send to and receive from the API.
- `Services` contain application behaviour behind interfaces.
- `Program.cs` connects HTTP routes to those services.
- `Tests` call the services directly, so the business behaviour is easy to understand.

## Run locally

Install the [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0), then run:

```bash
dotnet restore
dotnet run --project src/Halcyrobe.Api
```

Open the Swagger page shown in the terminal, usually:

```text
http://localhost:5132/swagger
```

Run the tests:

```bash
dotnet test
```

## Try the API

List all products:

```bash
curl http://localhost:5132/api/products
```

Filter the catalogue:

```bash
curl "http://localhost:5132/api/products?category=Jacket&maxPrice=150"
```

Copy a product ID from the response, then create a try-on job:

```bash
curl -X POST http://localhost:5132/api/try-ons \
  -H "Content-Type: application/json" \
  -d '{
    "productId": "11111111-1111-1111-1111-111111111111",
    "personImageUrl": "https://example.com/person.jpg"
  }'
```

The response has a `jobId`. Retrieve it with:

```bash
curl http://localhost:5132/api/try-ons/JOB_ID
```

## The C# concepts to learn

### 1. Records and enums

`Product` and `TryOnJob` are records: concise immutable data types. `TryOnStatus` is an enum, so job state cannot be an arbitrary string.

### 2. Interfaces and dependency injection

`IProductCatalog` and `ITryOnJobService` describe behaviour without choosing an implementation. `Program.cs` registers their in-memory implementations. ASP.NET Core supplies those services to each endpoint.

### 3. LINQ

`InMemoryProductCatalog` uses LINQ to apply optional filters. Read the method from top to bottom and observe how each filter adds a new condition.

### 4. Validation and explicit results

`InMemoryTryOnJobService.Create` returns `TryOnJobCreationResult` rather than throwing for expected user errors. The endpoint converts success into HTTP 201 and validation failures into HTTP 400 or 404.

### 5. Unit tests

The tests use Arrange, Act, Assert:

1. Arrange the service and input.
2. Act by calling one method.
3. Assert the returned value or error.

## A weekend learning plan

### Saturday morning

1. Run the API and open Swagger.
2. Read `Program.cs` and call each endpoint.
3. Set a breakpoint in one endpoint and follow the request into a service.

### Saturday afternoon

1. Read the models and service interfaces.
2. Add a fourth sample product.
3. Add a `colour` filter to `IProductCatalog` and its tests.

### Sunday morning

1. Read the try-on service and its tests.
2. Explain why the existing validation rejects `file:` URLs as well as malformed text.
3. Add an image-file extension rule by writing a failing test first, then make the test pass.

### Sunday afternoon

1. Replace one in-memory collection with a repository interface.
2. Explain aloud why the endpoint depends on an interface.
3. Write down how a real AI provider, database, retry policy, and background worker would fit without changing the HTTP contract.

## Current limitations

- Data is held in memory and resets when the API restarts.
- Try-on jobs remain queued; no external AI provider is called.
- Authentication, persistent storage, file uploads, and rate limits are intentionally out of scope.

These are deliberate boundaries, not hidden unfinished work. A sensible next step is to add a provider interface and background worker, then persist products and jobs with Entity Framework Core and PostgreSQL.

