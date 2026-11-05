using Halcyrobe.Api.Contracts;
using Halcyrobe.Api.Models;

namespace Halcyrobe.Api.Services;

public interface ITryOnJobService
{
    TryOnJobCreationResult Create(CreateTryOnRequest request);

    TryOnJob? GetById(Guid id);
}

public sealed record TryOnJobCreationResult(
    TryOnJob? Job,
    TryOnJobErrorCode? ErrorCode = null,
    string? Error = null)
{
    public static TryOnJobCreationResult Success(TryOnJob job) => new(job);

    public static TryOnJobCreationResult Failure(TryOnJobErrorCode code, string error) =>
        new(null, code, error);
}

public enum TryOnJobErrorCode
{
    ProductNotFound,
    InvalidImageUrl
}

