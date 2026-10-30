namespace hospitalBillingManagement.DTOs;

public class UpdatePatientRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public long MobileNo { get; set; }
    public decimal TotalAmountSpent { get; set; }
    public bool IsPaid { get; set; }
    public decimal PendingAmount { get; set; }
    public string? UpiId { get; set; }
}
