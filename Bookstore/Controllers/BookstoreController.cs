using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Bookstore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using PagedList;

namespace Bookstore.Controllers
{
    public class BookstoreController: Controller
    {
        private readonly BookstoreContext _context;
        private readonly BookstoreDAL bd = new BookstoreDAL();
        public BookstoreController(BookstoreContext context)
        {
            _context = context;

        }
        public async Task<IActionResult> SearchIndex()
        {
           
            List<Books> bookList = await bd.GetBooks();
            return View(bookList);
        }
        public async Task<IActionResult> IndividualBook(int id)
        {
            Books foundBook = await bd.GetBook(id);
            return View(foundBook);
        }

        public async Task<IActionResult> AddToCart(int id, int quantity)
        {
            string cartJson = HttpContext.Session.GetString("CartList");
            List<Cart> CartList = new List<Cart>();

            if (cartJson == null)
            {
                CartList.Clear();
            }
            else
            {
                CartList = JsonSerializer.Deserialize<List<Cart>>(cartJson);
            }
            
            Books foundBook = await bd.GetBook(id);

            Cart cartbook = new Cart(foundBook, quantity);
            //Add found book to the cart list and return user to the list of books
            if (CartList.Exists(x => x.Book.Title == foundBook.Title) == false)
            {
                if (quantity <= foundBook.Quantity)
                {
                   
                    CartList.Add(cartbook);

                    //resave the CartList with the new book in it
                    string cartListJson = JsonSerializer.Serialize(CartList);
                    HttpContext.Session.SetString("CartList", cartListJson);
                    return RedirectToAction("Cart");
                }
            }

            return View("CartError");
        }

        public async Task<IActionResult> Cart()
        {
            string cartJson = HttpContext.Session.GetString("CartList");
            List<Cart> CartList = new List<Cart>();

            if (cartJson != null)
            {
                CartList = JsonSerializer.Deserialize<List<Cart>>(cartJson);
            }

            if (CartList == null)
            {
                return View("SearchIndex");
            }

            return View(CartList);
        }
    }
}
