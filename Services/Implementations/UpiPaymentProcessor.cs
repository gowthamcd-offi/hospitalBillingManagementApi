using hospitalBillingManagement.Models;
using hospitalBillingManagement.Services.Interfaces;

namespace hospitalBillingManagement.Services.Implementations;

public class UpiPaymentProcessor : IPaymentProcessor
{
    public bool ProcessPayment(Patient patient, string paymentDetail, decimal amount)
    {
        if (string.IsNullOrWhiteSpace(paymentDetail)) return false;
        patient.UpiId = paymentDetail;
        patient.IsPaid = true;
        patient.TotalAmountSpent += patient.PendingAmount;
        patient.PendingAmount = 0.0m;
        patient.PaymentDate = DateTime.Now;
        return true;
    }
}