using hospitalBillingManagement.Models;

namespace hospitalBillingManagement.Repositories.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<Product?> GetByProductIdAsync(int productId);
    Task<IEnumerable<Product>> GetByProductIdsAsync(params int[] productIds);
}