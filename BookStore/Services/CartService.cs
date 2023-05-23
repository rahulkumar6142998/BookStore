using BookStore.Models;
using BookStore.Services.Interface;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Services
{
    public class CartService
    {
        private readonly IMongoCollection<BookModel> _books;
        private readonly IMongoCollection<CartModel> _cart;

        public CartService(IDataConnection dataConnection)
        {
            var client = new MongoClient(dataConnection.ConnectionString);
            var database = client.GetDatabase(dataConnection.DatabaseName);

            _books = database.GetCollection<BookModel>("Books");
            _cart = database.GetCollection<CartModel>("Cart");
        }

        public BookModel GetBookById(string id)
        {
            return _books.Find(b => b.Id == id).FirstOrDefault();
        }

        public List<BookModel> GetAllBooks()
        {
            return _books.Find(b => true).ToList();
        }

        public CartModel GetCartByUserId(string userId)
        {
            return _cart.Find(c => c.UserId == userId).FirstOrDefault();
        }

        public void AddBookToCart(string userId, string bookId)
        {
            var cart = _cart.Find(c => c.UserId == userId).FirstOrDefault();

            if (cart == null)
            {
                cart = new CartModel
                {
                    UserId = userId,
                    Products = new List<BookModel> { GetBookById(bookId) }
                };

                _cart.InsertOne(cart);
            }
            else
            {
                cart.Products.Add(GetBookById(bookId));

                var update = Builders<CartModel>.Update.Set(c => c.Products, cart.Products);
                _cart.UpdateOne(c => c.UserId == userId, update);
            }
        }

        public void DeleteBookFromCart(string bookId)
        {
            var update = Builders<CartModel>.Update.PullFilter(c => c.Products, p => p.Id == bookId);

            _cart.UpdateMany(c => true, update);
        }

        public void ClearCart(string user)
        {
            var filter = Builders<CartModel>.Filter.Eq("UserId", user);
            var cart = _cart.Find(filter).FirstOrDefault();
            _cart.DeleteOne(filter);


        }


    }

}
