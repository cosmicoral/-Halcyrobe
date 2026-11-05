using Halcyrobe.Api.Models;

namespace Halcyrobe.Api.Services;

public sealed class InMemoryProductCatalog : IProductCatalog
{
    private static readonly IReadOnlyList<Product> Products =
    [
        new(
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            "Northstar",
            "Everyday Denim Jacket",
            "Jacket",
            89.00m,
            "GBP",
            "https://images.example.com/denim-jacket.jpg",
            "https://shop.example.com/denim-jacket"),
        new(
            Guid.Parse("22222222-2222-2222-2222-222222222222"),
            "Lumen",
            "Silk Evening Dress",
            "Dress",
            145.00m,
            "GBP",
            "https://images.example.com/silk-dress.jpg",
            "https://shop.example.com/silk-dress"),
        new(
            Guid.Parse("33333333-3333-3333-3333-333333333333"),
            "Northstar",
            "Relaxed Cotton Shirt",
            "Shirt",
            54.00m,
            "GBP",
            "https://images.example.com/cotton-shirt.jpg",
            "https://shop.example.com/cotton-shirt")
    ];

    public IReadOnlyList<Product> GetAll(
        string? brand = null,
        string? category = null,
        decimal? maxPrice = null)
    {
        IEnumerable<Product> query = Products;

        if (!string.IsNullOrWhiteSpace(brand))
        {
            query = query.Where(product =>
                product.Brand.Equals(brand.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(product =>
                product.Category.Equals(category.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        if (maxPrice is not null)
        {
            query = query.Where(product => product.Price <= maxPrice.Value);
        }

        return query.OrderBy(product => product.Price).ToList();
    }

    public Product? GetById(Guid id) => Products.SingleOrDefault(product => product.Id == id);
}

