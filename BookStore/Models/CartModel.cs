using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class CartModel
    {
            [BsonId]
            public ObjectId Id { get; set; }

            [BsonElement("UserId")]
            public string UserId { get; set; }

            [BsonElement("Products")]
            public List<BookModel> Products { get; set; }
        
    }
}
