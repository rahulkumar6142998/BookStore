using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class LoginModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required]
        [EmailAddress]
        [BsonElement("Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [BsonElement("Password")]
        public string Password { get; set; }

        public bool? LoginStatus { get; set; }

        public string ResetPasswordCode { get; set; }

        [BsonElement("Valid Status")]
        public bool ValidStatus { get; set; }
    }
}
