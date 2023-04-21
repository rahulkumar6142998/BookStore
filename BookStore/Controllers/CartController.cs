using BookStore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    public class CartController : Controller
    {
        private readonly CartService _cartService;

        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost]
        public IActionResult AddToCart (string bookId)
        {
            var userId = HttpContext.Session.GetString("Id");
            _cartService.AddBookToCart(userId, bookId);
            return RedirectToAction("Index", "Home");
        }
    }
}
