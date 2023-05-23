using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("id")]
        public string Id { get; set; }

        [BsonElement("orderDate")]
        public DateTime OrderDate { get; set; }

        [BsonElement("items")]
        public CartModel Items { get; set; }

        [BsonElement("totalAmount")]
        public decimal TotalAmount { get; set; }
    }
}
