namespace Halcyrobe.Api.Models;

public sealed record Product(
    Guid Id,
    string Brand,
    string Name,
    string Category,
    decimal Price,
    string Currency,
    string ImageUrl,
    string ShoppingUrl);

