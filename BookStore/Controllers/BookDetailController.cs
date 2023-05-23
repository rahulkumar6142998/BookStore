using BookStore.Models;
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

        [HttpPost]
        public IActionResult SubmitReview(ReviewModel review)
        {
         
            var id = review.BookId;

            if (review.Comment != null)
            {
                // Validate the review and perform necessary checks
                if (!ModelState.IsValid)
                {

                    _bookDetailService.Review(review);


                }
            }
           

            // Redirect the user to the book details page or show a success message
            return RedirectToAction("Details", "BookDetail", new { BookId = review.BookId });
        }

    }
}
