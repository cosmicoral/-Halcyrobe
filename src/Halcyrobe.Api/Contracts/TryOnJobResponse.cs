using Halcyrobe.Api.Models;

namespace Halcyrobe.Api.Contracts;

public sealed record TryOnJobResponse(
    Guid JobId,
    Guid ProductId,
    string PersonImageUrl,
    TryOnStatus Status,
    DateTimeOffset CreatedAt)
{
    public static TryOnJobResponse From(TryOnJob job) =>
        new(job.Id, job.ProductId, job.PersonImageUrl, job.Status, job.CreatedAt);
}

