using hospitalBillingManagement.Data;
using hospitalBillingManagement.Models;
using hospitalBillingManagement.Repositories.Interfaces;
using MongoDB.Driver;

namespace hospitalBillingManagement.Repositories.Implementations;

public class ProductRepository : IProductRepository
{
    private readonly IMongoCollection<Product> _collection;

    public ProductRepository(MongoDbContext context)
    {
        _collection = context.Products;
        SeedInitialProductsAsync().Wait();
    }

    public async Task<Product> AddAsync(Product product)
    {
        await _collection.InsertOneAsync(product);
        return product;
    }

    public async Task<bool> UpdateAsync(Product product)
    {
        var result = await _collection.ReplaceOneAsync(p => p.Id == product.Id, product);
        return result.ModifiedCount > 0;
    }

    public async Task<Product?> GetByProductIdAsync(int productId)
    {
        return await _collection.Find(p => p.ProductId == productId).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Product>> GetByProductIdsAsync(params int[] productIds)
    {
        return await _collection.Find(p => productIds.Contains(p.ProductId)).ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    private async Task SeedInitialProductsAsync()
    {
        var count = await _collection.CountDocumentsAsync(_ => true);
        if (count == 0)
        {
            var initialProducts = new List<Product>
            {
                new Product { ProductId = 1, Name = "Paracetamol 500mg", Rate = 15.50m },
                new Product { ProductId = 2, Name = "Amoxicillin 250mg", Rate = 45.00m },
                new Product { ProductId = 3, Name = "Ibuprofen 400mg", Rate = 25.00m },
                new Product { ProductId = 4, Name = "Cough Syrup 100ml", Rate = 85.00m },
                new Product { ProductId = 5, Name = "Vitamin C Tablets", Rate = 30.00m }
            };

            await _collection.InsertManyAsync(initialProducts);
        }
    }
}
