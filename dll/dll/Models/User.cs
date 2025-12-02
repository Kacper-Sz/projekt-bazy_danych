using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dll.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("firstName")]
        public string FirstName { get; set; } = null!;

        [BsonElement("lastName")]
        public string LastName { get; set; } = null!;

        [BsonElement("email")]
        public string Email { get; set; } = null!;

        [BsonElement("role")]
        public string Role { get; set; } = "customer";

        [BsonElement("password")]
        public string Password { get; set; } = null!;

        [BsonElement("phoneNumber")]
        public string PhoneNumber { get; set; } = null!;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }
    }
}
