using hospitalBillingManagement.Data;
using hospitalBillingManagement.Models;
using hospitalBillingManagement.Repositories.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace hospitalBillingManagement.Repositories.Implementations;

public class PatientRepository : IPatientRepository
{
    private readonly IMongoCollection<Patient> _collection;

    public PatientRepository(MongoDbContext context)
    {
        _collection = context.Patients;
    }

    public async Task<Patient> AddAsync(Patient patient)
    {
        await _collection.InsertOneAsync(patient);
        return patient;
    }

    public async Task<bool> UpdateAsync(Patient patient)
    {
        var filter = Builders<Patient>.Filter.Eq(p => p.Id, patient.Id);
        var update = Builders<Patient>.Update
            .Set(p => p.PurchasedProducts, patient.PurchasedProducts)
            .Set(p => p.PendingAmount, patient.PendingAmount)
            .Set(p => p.TotalAmountSpent, patient.TotalAmountSpent)
            .Set(p => p.UpiId, patient.UpiId)
            .Set(p => p.IsPaid, patient.IsPaid)
            .Set(p => p.PaymentDate, patient.PaymentDate);

        var result = await _collection.UpdateOneAsync(filter, update);
        return result.ModifiedCount > 0;
    }

    public async Task<Patient?> SearchAsync(string identifier)
    {
        identifier = identifier.Trim();
        if (long.TryParse(identifier, out long mobile))
        {
            return await _collection.Find(p => p.MobileNo == mobile).FirstOrDefaultAsync();
        }
        if (identifier.Contains('@'))
        {
            return await _collection.Find(p => p.Email == identifier).FirstOrDefaultAsync();
        }
        var parts = identifier.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length >= 2)
        {
            string firstName = parts[0].Trim().ToLower();
            string lastName = string.Join(" ", parts.Skip(1)).Trim().ToLower();
            return await _collection.Find(p => p.FirstName.Trim().ToLower() == firstName && p.LastName.Trim().ToLower() == lastName).FirstOrDefaultAsync();
        }
        if (parts.Length == 1)
        {
            string firstName = parts[0].Trim().ToLower();
            return await _collection.Find(p => p.FirstName.Trim().ToLower() == firstName).FirstOrDefaultAsync();
        }
        return null;
    }

    public async Task<Patient[]> GetByProductIdAsync(int productId)
    {
        var filter = Builders<Patient>.Filter.ElemMatch(p => p.PurchasedProducts, prod => prod.ProductId == productId);
        return (await _collection.Find(filter).ToListAsync()).ToArray();
    }

    public async Task<IEnumerable<Patient>> GetAllAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }
}
