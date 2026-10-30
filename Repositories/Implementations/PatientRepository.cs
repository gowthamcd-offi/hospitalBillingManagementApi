using hospitalBillingManagement.Data;
using hospitalBillingManagement.Models;
using hospitalBillingManagement.Repositories.Generic;
using hospitalBillingManagement.Repositories.Interfaces;
using MongoDB.Driver;

namespace hospitalBillingManagement.Repositories.Implementations;

public class PatientRepository : GenericRepository<Patient>, IPatientRepository
{
    public PatientRepository(MongoDbContext context) : base(context, "Patients")
    {
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
}
