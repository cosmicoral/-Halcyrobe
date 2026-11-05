using Halcyrobe.Api.Models;

namespace Halcyrobe.Api.Services;

public interface IProductCatalog
{
    IReadOnlyList<Product> GetAll(string? brand = null, string? category = null, decimal? maxPrice = null);

    Product? GetById(Guid id);
}

