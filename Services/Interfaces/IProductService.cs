using hospitalBillingManagement.Models;

namespace hospitalBillingManagement.Services.Interfaces;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int productId);
    Task<Product[]> GetByIdAsync(params int[] productIds);
}
