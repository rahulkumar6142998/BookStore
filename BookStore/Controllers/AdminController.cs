using BookStore.Models;
using BookStore.Services;
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

        public IActionResult Login()
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
                    return RedirectToAction("Index", "Home");
                }
            }
            //TODO: Validation message need to be shown
            return View();

        }

        public IActionResult AddBook()
        {
           

            
            return View( new BookModel());
        }

        [HttpPost]
        public IActionResult AddBook(BookModel book)
        {
            if (ModelState.IsValid)
            {
                adminService.AddBook(book);

                return RedirectToAction(nameof(AllBooks));
            }

            return View(book);
           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(string id)
        {
            if (ModelState.IsValid)
            {
                adminService.DeleteBook(id);
            }
            return RedirectToAction(nameof(AllBooks));

        }

        public IActionResult AllBooks()
        {
            
            var books = adminService.GetAllBooks();

            return View(books);
        }


    }
}
