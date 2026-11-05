namespace Halcyrobe.Api.Contracts;

public sealed record CreateTryOnRequest(Guid ProductId, string PersonImageUrl);

