using hospitalBillingManagement.Models;

namespace hospitalBillingManagement.Services.Interfaces;

public interface IBillingService
{
    Task<Patient?> GetBillDetailsAsync(string identifier);
    Task<bool> SettleBillAsync(string identifier, string paymentDetail, IPaymentProcessor paymentProcessor);
}
