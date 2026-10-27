using hospitalBillingManagement.Models;

namespace hospitalBillingManagement.Services.Interfaces
{
    public interface IPaymentProcessor
    {
        bool ProcessPayment(Patient patient, string paymentDetail, decimal amount);
    }
}