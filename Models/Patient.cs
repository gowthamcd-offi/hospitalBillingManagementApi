using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace hospitalBillingManagement.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Rate { get; set; }
    }

    public class Patient
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public long MobileNo { get; set; }
        public decimal TotalAmountSpent { get; set; }
        public bool IsPaid { get; set; }
        public List<CartItem> PurchasedProducts { get; set; } = new();
        public DateTime PaymentDate { get; set; }
        public string? UpiId { get; set; }
        public decimal PendingAmount { get; set; }
    }
}