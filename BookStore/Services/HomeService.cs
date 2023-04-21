using BookStore.Models;
using BookStore.Services.Interface;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Services
{
    public class HomeService
    {
        private readonly IMongoCollection<BookModel> _books;

        public HomeService(IDataConnection dataConnection)
        {
            var client = new MongoClient(dataConnection.ConnectionString);
            var database = client.GetDatabase(dataConnection.DatabaseName);
           
            _books = database.GetCollection<BookModel>("Books");
        }

        public List<BookModel> SearchBooks(string query)
        {
            var keys = Builders<BookModel>.IndexKeys.Text(x => x.Title);
            var model = new CreateIndexModel<BookModel>(keys);
            _books.Indexes.CreateOne(model);

            var filter = Builders<BookModel>.Filter.Text(query);
            var searchResults = _books.Find(filter).ToList();
            return searchResults;




        }
    }
}
