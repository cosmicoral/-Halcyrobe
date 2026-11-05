namespace Halcyrobe.Api.Models;

public sealed record TryOnJob(
    Guid Id,
    Guid ProductId,
    string PersonImageUrl,
    TryOnStatus Status,
    DateTimeOffset CreatedAt);

public enum TryOnStatus
{
    Queued,
    Processing,
    Completed,
    Failed
}

