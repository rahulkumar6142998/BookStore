using BookStore.Common;
using BookStore.Models;
using BookStore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BookStore.Controllers
{
    public class AdminController: Controller
    {
        private readonly AdminService adminService;

        public AdminController(AdminService adminService)
        {
            this.adminService = adminService;
        }

        public IActionResult Login(string encryptEmail)
        {
            return View();
        }

        [HttpPost]
        public  IActionResult Login(Admin admin)
        {
            if (ModelState.IsValid)
            {
                var result = adminService.validateCredential(admin);

                if(result!=null)
                {
                     HttpContext.Session.SetString("userName", admin.Username.ToString());
                    string userName = HttpContext.Session.GetString("userName");
                    var encryptEmail = Base64.Base64Encode(userName);
                    return RedirectToAction("AllBooks","Admin");
                }
            }
            //TODO: Validation message need to be shown
            return View();

        }

        public IActionResult AddBook(string encryptEmail)
        {
            if(encryptEmail==null)
            {
                return RedirectToAction("Login");
            }
            return View(new BookModel());
        }

        [HttpPost]
        public IActionResult AddBook(BookModel book, string encryptEmail)
        {
            if (encryptEmail == null)
            {
                return RedirectToAction("Login");
            }
            if (ModelState.IsValid)
            {
                book.QuantityLeft = book.Quantity;
                adminService.AddBook(book);

                return RedirectToAction(nameof(AllBooks));
            }

            return View(book);
           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(string id, string encryptEmail)
        {
            if (encryptEmail == null)
            {
                return RedirectToAction("Login");
            }
            if (ModelState.IsValid)
            {
                adminService.DeleteBook(id);
            }
            return RedirectToAction(nameof(AllBooks));

        }

        public IActionResult AllBooks(string encryptEmail)
        {
            
            var books = adminService.GetAllBooks();

            return View(books);
        }

        public IActionResult SearchByISBN(string encryptEmail)
        {
            if (encryptEmail == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        [HttpPost]
        public IActionResult SearchByISBN(string isbn, string encryptEmail)
        {
            if (encryptEmail == null)
            {
                return RedirectToAction("Login");
            }
            if (!string.IsNullOrEmpty(isbn))
            {
                if (!adminService.FindBook(isbn))
                {
                    try
                    {
                        var book = adminService.GetBookByIsbn(isbn);
                        if (book != null)
                        {
                            BookModel item = new BookModel
                            {
                                Author = book.Authors?.FirstOrDefault(),
                                Description = book.Description,
                                Title = book.Title,
                                Genre = book.Categories?.FirstOrDefault(),
                                ISBN = isbn,
                                Image = book.ImageLinks?.Thumbnail,
                                PageCount = book.PageCount,
                                Publisher = book.Publisher,
                                Quantity = 100,
                                QuantityLeft = 100


                            };

                            return View("AddBookAPI", item);
                        }
                    }
                    catch (Google.GoogleApiException ex)
                    {
                        ModelState.AddModelError("isbn", "Error fetching book details: " + ex.Message);
                    }
                }
            }

            return View();
        }


        [HttpGet]
        public IActionResult AllOrder()
        {
            var o = adminService.AllOrder();

            List<UserOrder> userOrder = new List<UserOrder>();
            foreach (var order in o)
            {
                List<string> productTitles = order.Items.Products.Select(p => p.Title).ToList();
                var totalTitles = productTitles.Count();

                foreach (var title in productTitles)
                {
                    UserOrder userOrderItem = new UserOrder
                    {

                        OrderDate = order.OrderDate,
                        Title = title,
                        TotalAmount = order.TotalAmount
                    };
                    userOrder.Add(userOrderItem);
                }


            }

            return View(userOrder);
        }

        [HttpGet]
        public IActionResult LogOut()
        {

          
            HttpContext.Session.Remove("userName");
            HttpContext.Session.Clear();

            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }

    }
}
