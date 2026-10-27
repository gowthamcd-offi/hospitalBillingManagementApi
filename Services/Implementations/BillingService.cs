using hospitalBillingManagement.Models;
using hospitalBillingManagement.Repositories.Interfaces;
using hospitalBillingManagement.Services.Interfaces;

namespace hospitalBillingManagement.Services.Implementations;

public class BillingService : IBillingService
{
    private readonly IPatientRepository _patientRepository;

    public BillingService(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task<Patient?> GetBillDetailsAsync(string identifier)
    {
        return await _patientRepository.SearchAsync(identifier);
    }

    public async Task<bool> SettleBillAsync(string identifier, string paymentDetail, IPaymentProcessor paymentProcessor)
    {
        var patient = await _patientRepository.SearchAsync(identifier);
        if (patient == null) return false;
        bool success = paymentProcessor.ProcessPayment(patient, paymentDetail, patient.PendingAmount);
        if (success) await _patientRepository.UpdateAsync(patient);
        return success;
    }
}