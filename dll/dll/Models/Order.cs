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
        public ObjectId Id { get; set; }

        [BsonElement("customerId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId CustomerId { get; set; }

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
        [BsonElement("productId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId ProductId { get; set; }

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
