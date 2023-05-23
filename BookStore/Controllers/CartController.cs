using BookStore.Models;
using BookStore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    public class CartController : Controller
    {
        private readonly CartService _cartService;
        public readonly UserService _userService;
        private static decimal totalPrice;
        private IConfiguration configuration;

        public CartController(CartService cartService, UserService userService, IConfiguration configuration)
        {
            _cartService = cartService;
            _userService = userService;
            this.configuration = configuration;
        }

        [HttpPost]
        public IActionResult AddToCart(string bookId)
        {
            var userId = HttpContext.Session.GetString("Id");
            _cartService.AddBookToCart(userId, bookId);
            return RedirectToAction("Details", "BookDetail", new { BookId = bookId });
        }

        public IActionResult Cart(string encryptEmail)
        {

            var user = HttpContext.Session.GetString("Id");
            var cartBook = _cartService.GetCartByUserId(user);
            if (cartBook != null)
            {
                var b = cartBook.Products;
                return View(b);
            }

            return View();
        }

        [HttpPost]
        public IActionResult Remove(string BookId)
        {

            _cartService.DeleteBookFromCart(BookId);
            return RedirectToAction("Cart");
        }

        [HttpGet]
        public IActionResult Checkout(decimal price)
        {
            totalPrice = price;
            return View("Checkout");
        }

        [HttpPost]
        public IActionResult Checkout(CheckoutModel model, string value)
        {
            int val = Convert.ToInt32(value);
            if (val == 1)
            {
                var user = HttpContext.Session.GetString("Id");
                var cartBook = _cartService.GetCartByUserId(user);
                var book = new List<BookModel>();
                var link = new List<Links>();

                var order = new Order();

                order.OrderDate = DateTime.Today;
                order.Items = cartBook;
                order.TotalAmount = totalPrice;



                foreach (var item in cartBook.Products)
                {
                    book.Add(item);

                }

                foreach (var item in book)
                {

                    var temp = new Links();
                    temp.Images = item.Image;
                    temp.Titles = item.Title;

                    link.Add(temp);

                }

                var result = GenerateEmailBody(model, link);

                if (result == 1)
                {
                    _userService.InsertOrder(order);
                    _cartService.ClearCart(user);
                    return RedirectToRoute(new { controller = "Cart", action = "ThankYou" });
                }
            }
            return View();

        }

        public IActionResult ThankYou()
        {
            return View("ThankYou");
        }

        public int GenerateEmailBody(CheckoutModel model, List<Links> links)
        {
            string orderId = Guid.NewGuid().ToString();
            var customerName = model.FirstName + " " + model.LastName;
            var mobileNumber = model.MobileNumber;
            string fullAddress = null;
            if (!string.IsNullOrEmpty(model.Address2))
            {
                fullAddress += ", " + model.Address2;
            }
            fullAddress += ", " + model.Postcode + ", " + model.State + ", " + model.City;
            // Calculate the delivery date
            DateTime deliveryDate = DateTime.Today.AddDays(new Random().Next(5, 11));

            string emailBody = @"
        <!DOCTYPE html>
        <html>
        <head>
            <style>
                body {
                    font-family: Arial, sans-serif;
                }

                .email-container {
                    max-width: 600px;
                    margin: 0 auto;
                    padding: 20px;
                    background-color: #f5f5f5;
                    border-radius: 5px;
                }

                .book-image {
                    width: 100px;
                    height: auto;
                    margin-bottom: 10px;
                }

                .book-title {
                    font-size: 18px;
                    font-weight: bold;
                    margin-bottom: 5px;
                }

                .address {
                    font-size: 14px;
                    margin-bottom: 10px;
                }

                .customer-details {
                    font-size: 16px;
                    margin-bottom: 10px;
                }

                .delivery-details {
                    font-size: 16px;
                    font-weight: bold;
                    margin-top: 20px;
                }
            </style>
        </head>
        <body>
           <div class='email-container'>
                <h2>Your Order Confirmation</h2>
                <p class='customer-details'>Order ID: " + orderId + @"</p>
                <p class='customer-details'>Name: " + customerName + @"</p>
                <p class='customer-details'>Mobile Number: " + mobileNumber + @"</p>
                <p class='address'>Address: " + fullAddress + @"</p>";

            foreach (var book in links)
            {
                string bookEntry = $@"
                <div class='book'>
                    <img class='book-image' src='{book.Images}' alt='Book'>
                    <h3 class='book-title'>{book.Titles}</h3>
                </div>";

                emailBody += bookEntry;
            }

            emailBody += $@"
                <p class='delivery-details'>Estimated Delivery Date: {deliveryDate.ToString("MMMM dd, yyyy")}</p>
            </div>
        </body>
        </html>";

            string sellerEmailBody = @"
        <!DOCTYPE html>
        <html>
        <head>
            <style>
                body {
                    font-family: Arial, sans-serif;
                }

                .email-container {
                    max-width: 600px;
                    margin: 0 auto;
                    padding: 20px;
                    background-color: #f5f5f5;
                    border-radius: 5px;
                }

                .order-details {
                    font-size: 16px;
                    margin-bottom: 10px;
                }

                .delivery-details {
                    font-size: 16px;
                    font-weight: bold;
                    margin-top: 20px;
                }
            </style>
        </head>
        <body>
            <div class='email-container'>
                <h2>New Order Received</h2>
                <p class='order-details'>Order ID: " + orderId + @"</p>
                <p class='order-details'>Customer Name: " + customerName + @"</p>
                <p class='order-details'>Mobile Number: " + mobileNumber + @"</p>
                <p class='order-details'>Address: " + fullAddress + @"</p>";

            foreach (var book in links)
            {
                string bookEntry = $@"
                <div class='book'>
                    <img class='book-image' src='{book.Images}' alt='Book'>
                    <h3 class='book-title'>{book.Titles}</h3>
                </div>";

                sellerEmailBody += bookEntry;
            }

            sellerEmailBody += $@"
                <p class='delivery-details'>Estimated Delivery Date: {deliveryDate.ToString("MMMM dd, yyyy")}</p>
            </div>
        </body>
        </html>";

            // Send email to the customer
            // ...
            var mailSender = new MailSender(configuration);
            mailSender.Send(model.Email, configuration["Yandex:Username"], "Book  Order Confirmation", sellerEmailBody);
            // Send email to the seller
            // ...
            var mailSender2 = new MailSender(configuration);
            mailSender2.Send(configuration["Admin:Email"], configuration["Yandex:Username"], "New Order Recived", emailBody);
            return 1;

        }
    }
}
