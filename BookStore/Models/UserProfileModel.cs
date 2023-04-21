using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookStore.Models
{
    public class UserProfileModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        [BsonElement("FirstName")]
        public string FirstName { get; set; }

        [BsonElement("LastName")]
        public string LastName { get; set; }

        //public Address Address { get; set; }

        public string Phone { get; set; }

        //public List<ObjectId> Wishlist { get; set; }

        //public List<CartItem> Cart { get; set; }

        //public List<Order> Orders { get; set; }
    }
}
