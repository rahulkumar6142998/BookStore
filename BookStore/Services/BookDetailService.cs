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

        public BookDetailService(IDataConnection dataConnection)
        {
            var client = new MongoClient(dataConnection.ConnectionString);
            var database = client.GetDatabase(dataConnection.DatabaseName);
           
            _books = database.GetCollection<BookModel>("Books");
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
    }
}
