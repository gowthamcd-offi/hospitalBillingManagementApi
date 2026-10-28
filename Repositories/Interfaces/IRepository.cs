using MongoDB.Bson;

namespace hospitalBillingManagement.Repositories.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T> AddAsync(T entity);
    Task<bool> UpdateAsync(T entity);
    Task<IEnumerable<T>> GetAllAsync();
}
