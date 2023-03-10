using BookStore.Models;
using BookStore.Services.Interface;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Services
{
    public class AdminService:IAdminService
    {
        private readonly IMongoCollection<Admin> _admins;
        private readonly IMongoCollection<BookModel> _books;

        public AdminService(IDataConnection dataConnection)
        {
            var client = new MongoClient(dataConnection.ConnectionString);
            var database = client.GetDatabase(dataConnection.DatabaseName);
            _admins = database.GetCollection<Admin>("Admin");
            _books = database.GetCollection<BookModel>("Books");
        }

        public Admin validateCredential(Admin adminLoginModel)
        {
            try
            {
                var filter = Builders<Admin>.Filter.Eq(x => x.Username, adminLoginModel.Username);
                var result = _admins.Find(filter).FirstOrDefault();
               
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in validateCredential" + ex.Message);
            }
        }
        public void AddBook(BookModel book)
        {
            try
            {
                _books.InsertOne(book);
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public void DeleteBook(string id)
        {
            try
            {
                var filter = Builders<BookModel>.Filter.Eq(b => b.Id, id);
                var book = _books.Find(filter).FirstOrDefault();
                if (book != null)
                {
                    _books.DeleteOne(filter);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public List<BookModel> GetAllBooks()
        {
           
            try
            {
               return _books.Find(_ => true).ToList();
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine(ex.Message);
                throw;
            }
        }

    }
}
