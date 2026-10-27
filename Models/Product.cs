using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace hospitalBillingManagement.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Rate { get; set; }
    }
}