using MongoDB.Bson;

namespace hospitalBillingManagement.Repositories.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T> CreateAsync(T entity);
    Task<bool> UpdateAsync(T entity);
    Task<T?> GetByIdAsync(string id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<bool> DeleteAsync(string id);
}
