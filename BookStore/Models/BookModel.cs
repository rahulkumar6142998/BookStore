using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    [BsonIgnoreExtraElements]
    public class BookModel
    {
        
            [BsonId]
            [BsonRepresentation(BsonType.ObjectId)]
            public string Id { get; set; }

            [BsonElement("title")]
            public string Title { get; set; }

            [BsonElement("author")]
            public string Author { get; set; }

            [BsonElement("description")]
            public string Description { get; set; }

            [BsonElement("genre")]
            public string Genre { get; set; }

            [BsonElement("price")]
            public decimal Price { get; set; }

            [BsonElement("isbn")]
            public string ISBN { get; set; }

            [BsonElement("image")]
            public string Image { get; set; }
        
    }
}
