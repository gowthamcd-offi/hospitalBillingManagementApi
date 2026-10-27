using hospitalBillingManagement.Models;
using hospitalBillingManagement.Repositories.Interfaces;
using hospitalBillingManagement.Services.Interfaces;

namespace hospitalBillingManagement.Services.Implementations;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _productRepository.GetAllAsync();
    }

    public async Task<Product?> GetByIdAsync(int productId)
    {
        return await _productRepository.GetByProductIdAsync(productId);
    }

    public async Task<Product[]> GetByIdAsync(params int[] productIds)
    {
        return (await _productRepository.GetByProductIdsAsync(productIds)).ToArray();
    }
}