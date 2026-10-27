using hospitalBillingManagement.Models;

namespace hospitalBillingManagement.Services.Interfaces;

public interface IPatientService
{
    Task<Patient> CreateAsync(string firstName, string lastName, string email, long mobileNo);
    Task<bool> AddMedicineAsync(string identifier, int productId, int quantity);
    Task<Patient[]> GetPatientsByProductAsync(int productId);
    Task<Patient?> SearchAsync(string identifier);
    Task<IEnumerable<Patient>> GetAllAsync();
}