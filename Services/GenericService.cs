using MongoDB.Driver;

namespace hospitalBillingManagement.Services.Generic;

public abstract class GenericService<TRepository, TEntity> where TRepository : class
{
    protected readonly TRepository _repository;

    protected GenericService(TRepository repository)
    {
        _repository = repository;
    }
}
