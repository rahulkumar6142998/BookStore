using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    
        public class Address
        {
            [BsonId]
            [BsonRepresentation(BsonType.ObjectId)]
            public string Id { get; set; }

            [BsonElement("street")]
            public string Street { get; set; }

            [BsonElement("city")]
            public string City { get; set; }

            [BsonElement("state")]
            public string State { get; set; }

            [BsonElement("postalCode")]
            public string PostalCode { get; set; }

        
        }
}
