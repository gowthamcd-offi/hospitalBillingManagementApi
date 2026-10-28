using hospitalBillingManagement.Models;
using MongoDB.Driver;

namespace hospitalBillingManagement.Data;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(MongoDbSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        _database = client.GetDatabase(settings.DatabaseName);
    }

    public IMongoCollection<Patient> Patients => _database.GetCollection<Patient>("Patients");
    public IMongoCollection<Product> Products => _database.GetCollection<Product>("Products");
}
