using Halcyrobe.Api.Contracts;
using Halcyrobe.Api.Models;
using Halcyrobe.Api.Services;
using Xunit;

namespace Halcyrobe.Api.Tests;

public sealed class TryOnJobServiceTests
{
    private readonly InMemoryProductCatalog _catalog = new();

    [Fact]
    public void Create_ReturnsProductNotFound_ForUnknownProduct()
    {
        var service = new InMemoryTryOnJobService(_catalog);
        var request = new CreateTryOnRequest(Guid.NewGuid(), "https://example.com/person.jpg");

        var result = service.Create(request);

        Assert.Null(result.Job);
        Assert.Equal(TryOnJobErrorCode.ProductNotFound, result.ErrorCode);
    }

    [Theory]
    [InlineData("not-a-url")]
    [InlineData("file:///Users/test/person.jpg")]
    public void Create_ReturnsInvalidImageUrl_ForUnsupportedUrl(string imageUrl)
    {
        var service = new InMemoryTryOnJobService(_catalog);
        var productId = _catalog.GetAll().First().Id;

        var result = service.Create(new CreateTryOnRequest(productId, imageUrl));

        Assert.Null(result.Job);
        Assert.Equal(TryOnJobErrorCode.InvalidImageUrl, result.ErrorCode);
    }

    [Fact]
    public void Create_QueuesAndStoresAValidJob()
    {
        var service = new InMemoryTryOnJobService(_catalog);
        var productId = _catalog.GetAll().First().Id;

        var result = service.Create(
            new CreateTryOnRequest(productId, "https://example.com/person.jpg"));

        Assert.NotNull(result.Job);
        Assert.Equal(TryOnStatus.Queued, result.Job.Status);
        Assert.Equal(productId, result.Job.ProductId);
        Assert.Equal(result.Job, service.GetById(result.Job.Id));
    }
}

