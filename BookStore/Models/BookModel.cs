using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [BsonElement("publisher")]
        [Required(ErrorMessage = "Publisher is required")]
        public string Publisher { get; set; }

        [BsonElement("author")]
        [Required(ErrorMessage = "Author is required")]
        public string Author { get; set; }

        [BsonElement("description")]
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [BsonElement("genre")]
        [Required(ErrorMessage = "Genre is required")]
        public string Genre { get; set; }

        [BsonElement("price")]
        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [BsonElement("isbn")]
        [Required(ErrorMessage = "ISBN is required")]
        public string ISBN { get; set; }

        [BsonElement("image")]
        //[Required(ErrorMessage = "Image is required")]
        public string Image { get; set; }

        [BsonElement("pagecount")]
        [Required(ErrorMessage = "Page count is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Page count must be greater than 0")]
        public int PageCount { get; set; }

        [BsonElement("quantity")]
        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }

        [BsonElement("quantityleft")]
        //[Required(ErrorMessage = "Quantity left is required")]
        //[Range(0, int.MaxValue, ErrorMessage = "Quantity left must be greater than or equal to 0")]
        public int QuantityLeft { get; set; }

        public IEnumerable<ReviewModel> Reviews { get; set; }

        [BsonIgnore]
        public ReviewModel Review { get; set; }
    }
}
