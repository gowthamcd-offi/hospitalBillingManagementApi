using hospitalBillingManagement.Models;
using hospitalBillingManagement.Repositories.Interfaces;
using hospitalBillingManagement.Services.Interfaces;

namespace hospitalBillingManagement.Services.Implementations;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;
    private readonly IProductRepository _productRepository;

    public PatientService(IPatientRepository patientRepository, IProductRepository productRepository)
    {
        _patientRepository = patientRepository;
        _productRepository = productRepository;
    }

    public async Task<Patient> CreateAsync(string firstName, string lastName, string email, long mobileNo)
    {
        var patient = new Patient
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            MobileNo = mobileNo
        };

        return await _patientRepository.CreateAsync(patient);
    }

    public async Task<bool> AddMedicineAsync(string identifier, int productId, int quantity)
    {
        var patient = await _patientRepository.SearchAsync(identifier);
        var product = await _productRepository.GetByProductIdAsync(productId);
        if (patient == null || product == null || quantity <= 0) return false;

        var existingItem = patient.PurchasedProducts.FirstOrDefault(i => i.ProductId == productId);
        if (existingItem != null) existingItem.Quantity += quantity;
        else patient.PurchasedProducts.Add(new CartItem { ProductId = product.ProductId, ProductName = product.Name, Quantity = quantity, Rate = product.Rate });

        patient.PendingAmount += product.Rate * quantity;
        return await _patientRepository.UpdateAsync(patient);
    }

    public async Task<Patient[]> GetPatientsByProductAsync(int productId)
    {
        return await _patientRepository.GetByProductIdAsync(productId);
    }

    public async Task<Patient?> SearchAsync(string identifier)
    {
        return await _patientRepository.SearchAsync(identifier);
    }

    public async Task<IEnumerable<Patient>> GetAllAsync()
    {
        return await _patientRepository.GetAllAsync();
    }
}
