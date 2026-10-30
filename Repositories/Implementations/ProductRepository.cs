using hospitalBillingManagement.Data;
using hospitalBillingManagement.Models;
using hospitalBillingManagement.Repositories.Generic;
using hospitalBillingManagement.Repositories.Interfaces;
using MongoDB.Driver;

namespace hospitalBillingManagement.Repositories.Implementations;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    public ProductRepository(MongoDbContext context) : base(context, "Products")
    {
        SeedInitialProductsAsync().Wait();
    }

    public async Task<Product?> GetByProductIdAsync(int productId)
    {
        return await _collection.Find(p => p.ProductId == productId).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Product>> GetByProductIdsAsync(params int[] productIds)
    {
        return await _collection.Find(p => productIds.Contains(p.ProductId)).ToListAsync();
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
