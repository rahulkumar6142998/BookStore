using BookStore.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    public class BookDetailController: Controller
    {
        private readonly BookDetailService _bookDetailService;

        public BookDetailController(BookDetailService bookDetailService)
        {
            _bookDetailService = bookDetailService;
        }

       
        public IActionResult Details(string BookId)
        {
            var book = _bookDetailService.GetBookById(BookId);
            return View("Details",book);
        }
    }
}
