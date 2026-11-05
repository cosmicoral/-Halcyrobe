using Halcyrobe.Api.Services;
using Xunit;

namespace Halcyrobe.Api.Tests;

public sealed class ProductCatalogTests
{
    private readonly InMemoryProductCatalog _catalog = new();

    [Fact]
    public void GetAll_FiltersByBrandWithoutCaseSensitivity()
    {
        var products = _catalog.GetAll(brand: "northstar");

        Assert.Equal(2, products.Count);
        Assert.All(products, product => Assert.Equal("Northstar", product.Brand));
    }

    [Fact]
    public void GetAll_CombinesCategoryAndMaximumPriceFilters()
    {
        var products = _catalog.GetAll(category: "jacket", maxPrice: 100m);

        var product = Assert.Single(products);
        Assert.Equal("Everyday Denim Jacket", product.Name);
    }

    [Fact]
    public void GetAll_OrdersProductsByPrice()
    {
        var products = _catalog.GetAll();

        Assert.Equal(products.OrderBy(product => product.Price), products);
    }
}

