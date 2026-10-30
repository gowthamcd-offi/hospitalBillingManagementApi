namespace hospitalBillingManagement.DTOs;

public class CreateProductRequest
{
    public int ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Rate { get; set; }
}
