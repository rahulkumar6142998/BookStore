using BookStore.Models;
using BookStore.Services.Interface;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using Google.Apis.Books.v1;
using Google.Apis.Books.v1.Data;
using Google.Apis.Services;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using BookStore.Models.API;

namespace BookStore.Services
{
    public class AdminService:IAdminService
    {
        private readonly IMongoCollection<Admin> _admins;
        private readonly IMongoCollection<BookModel> _books;
        private readonly IMongoCollection<Order> _order;


        private readonly HttpClient _httpClient;

        public AdminService(IDataConnection dataConnection, IHttpClientFactory httpClientFactory)
        {
            var client = new MongoClient(dataConnection.ConnectionString);
            var database = client.GetDatabase(dataConnection.DatabaseName);
            _admins = database.GetCollection<Admin>("Admin");
            _books = database.GetCollection<BookModel>("Books");
            _order = database.GetCollection<Order>("Order");
            _httpClient = httpClientFactory.CreateClient();
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

        public VolumeInfoModel GetBookByIsbn(string isbn)
        {
            var response = _httpClient.GetAsync($"https://www.googleapis.com/books/v1/volumes?q=isbn:{isbn}").Result;

            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<BookApiResponseModel>(content);

                if (result.TotalItems > 0)
                {
                    return result.Items[0].VolumeInfo;
                }
            }

            return null;
        }

        public bool FindBook(string isbn)
        {
            return _books.Find(book => book.ISBN == isbn).Any();
        }

        public List<Order> AllOrder()
        {
            var filter = Builders<Order>.Filter.Empty;

           
            List<Order> allOrders =_order.Find(filter).ToList();

            return allOrders;
        }
    }


}

