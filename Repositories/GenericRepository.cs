using hospitalBillingManagement.Data;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace hospitalBillingManagement.Repositories.Generic;

public abstract class GenericRepository<T> where T : class
{
    protected readonly IMongoCollection<T> _collection;
    protected readonly string _collectionName;

    protected GenericRepository(MongoDbContext context, string collectionName)
    {
        _collection = context.Database.GetCollection<T>(collectionName);
        _collectionName = collectionName;
    }

    public async Task<T> CreateAsync(T entity)
    {
        await _collection.InsertOneAsync(entity);
        return entity;
    }

    public async Task<bool> UpdateAsync(string id, T entity)
    {
        var filter = Builders<T>.Filter.Eq("_id", id);
        var result = await _collection.ReplaceOneAsync(filter, entity);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> UpdateAsync(T entity)
    {
        var id = GetIdValue(entity);
        if (string.IsNullOrEmpty(id)) return false;
        
        var filter = Builders<T>.Filter.Eq("_id", id);
        var result = await _collection.ReplaceOneAsync(filter, entity);
        return result.ModifiedCount > 0;
    }

    public async Task<T?> GetByIdAsync(string id)
    {
        var filter = Builders<T>.Filter.Eq("_id", id);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var filter = Builders<T>.Filter.Eq("_id", id);
        var result = await _collection.DeleteOneAsync(filter);
        return result.DeletedCount > 0;
    }

    protected virtual string? GetIdValue(T entity)
    {
        var type = typeof(T);
        var idProperty = type.GetProperty("Id");
        if (idProperty != null)
        {
            var value = idProperty.GetValue(entity);
            return value?.ToString();
        }
        return null;
    }
}
