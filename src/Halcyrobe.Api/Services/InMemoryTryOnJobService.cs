using System.Collections.Concurrent;
using Halcyrobe.Api.Contracts;
using Halcyrobe.Api.Models;

namespace Halcyrobe.Api.Services;

public sealed class InMemoryTryOnJobService(IProductCatalog products) : ITryOnJobService
{
    private readonly ConcurrentDictionary<Guid, TryOnJob> _jobs = new();

    public TryOnJobCreationResult Create(CreateTryOnRequest request)
    {
        if (products.GetById(request.ProductId) is null)
        {
            return TryOnJobCreationResult.Failure(
                TryOnJobErrorCode.ProductNotFound,
                $"Product '{request.ProductId}' was not found.");
        }

        if (!Uri.TryCreate(request.PersonImageUrl, UriKind.Absolute, out var imageUri)
            || (imageUri.Scheme != Uri.UriSchemeHttp && imageUri.Scheme != Uri.UriSchemeHttps))
        {
            return TryOnJobCreationResult.Failure(
                TryOnJobErrorCode.InvalidImageUrl,
                "PersonImageUrl must be an absolute HTTP or HTTPS URL.");
        }

        var job = new TryOnJob(
            Guid.NewGuid(),
            request.ProductId,
            imageUri.ToString(),
            TryOnStatus.Queued,
            DateTimeOffset.UtcNow);

        _jobs[job.Id] = job;
        return TryOnJobCreationResult.Success(job);
    }

    public TryOnJob? GetById(Guid id) => _jobs.GetValueOrDefault(id);
}

