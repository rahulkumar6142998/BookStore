using BookStore.Models;
using BookStore.Services.Interface;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Services
{
    public class BookDetailService
    {
        private readonly IMongoCollection<BookModel> _books;
        private readonly IMongoCollection<ReviewModel> _review;

        public BookDetailService(IDataConnection dataConnection)
        {
            var client = new MongoClient(dataConnection.ConnectionString);
            var database = client.GetDatabase(dataConnection.DatabaseName);
           
            _books = database.GetCollection<BookModel>("Books");
            _review = database.GetCollection<ReviewModel>("Review");
        }

        public BookModel GetBookById(string id)
        {
            try
            {
                var objectId = new ObjectId(id);
                var filter = Builders<BookModel>.Filter.Eq("_id", objectId);
                var book =  _books.Find(filter).FirstOrDefault();
                return book;
            }
            catch(Exception ex)
            {
                throw new Exception("Error in GetBookById" + ex.Message);
            }
        }

        public void Review( ReviewModel reviewModel)
        {
            try
            {
                var objectId = new ObjectId(reviewModel.BookId);
                var filter = Builders<BookModel>.Filter.Eq("_id", objectId);
                var book = _books.Find(filter).FirstOrDefault();

                if (book != null)
                {
                    // Initialize the Reviews property if it's null
                    if (book.Reviews == null)
                    {
                        book.Reviews = new List<ReviewModel>();
                    }

                    // Add the new review to the book's Reviews collection
                    ((List<ReviewModel>)book.Reviews).Add(reviewModel);

                    // Update the book in the database
                    _books.ReplaceOne(filter, book);
                }


            }
            catch (Exception ex)
            {
                throw new Exception("Error in Review" + ex.Message);
            }
        }
    }
}
