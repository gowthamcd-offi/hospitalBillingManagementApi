using hospitalBillingManagement.Models;

namespace hospitalBillingManagement.Repositories.Interfaces;

public interface IPatientRepository : IRepository<Patient>
{
    Task<Patient?> SearchAsync(string identifier);
    Task<Patient[]> GetByProductIdAsync(int productId);
}