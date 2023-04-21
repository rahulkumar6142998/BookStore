using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class UserRegistrationModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Email")]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [BsonElement("Password")]
        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [BsonElement("ConfirmedPassword")]
        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        public string ConfirmedPassword { get; set; }

        [BsonElement("FirstName")]
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [BsonElement("LastName")]
       
        public string LastName { get; set; }

        [BsonElement("CreatedOn")]
        public DateTime Created { get; set; }

        [BsonElement("Valid Status")]
        public bool ValidStatus { get; set; }
    }
}
