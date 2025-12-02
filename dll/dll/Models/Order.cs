using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dll.Models
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("customerId")]
        public string CustomerId { get; set; } = null!;

        [BsonElement("items")]
        public List<OrderItem> Items { get; set; } = new();

        [BsonElement("status")]
        public string Status { get; set; } = "processing";

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("totalAmount")]
        public decimal TotalAmount { get; set; }

        [BsonElement("deliveryAddress")]
        public DeliveryAddress DeliveryAddress { get; set; } = new();

        [BsonElement("paymentMethod")]
        public string PaymentMethod { get; set; } = null!;
    }

    public class OrderItem
    {
        [BsonElement("productName")]
        public string ProductName { get; set; } = null!;

        [BsonElement("quantity")]
        public int Quantity { get; set; }
    }

    public class DeliveryAddress
    {
        [BsonElement("city")]
        public string City { get; set; } = null!;

        [BsonElement("street")]
        public string Street { get; set; } = null!;

        [BsonElement("postalCode")]
        public string PostalCode { get; set; } = null!;
    }
}
