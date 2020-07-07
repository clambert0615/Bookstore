using Bookstore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace Bookstore.Controllers
{
    public class BookstoreController : Controller
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

        public IActionResult Cart()
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

            return RedirectToAction("SaveCart", CartList);
        }
        [Authorize]
        public IActionResult SaveCart()
        {
            string cartJson = HttpContext.Session.GetString("CartList");
            List<Cart> CartList = new List<Cart>();

            if (cartJson != null)
            {
                CartList = JsonSerializer.Deserialize<List<Cart>>(cartJson);
            }

            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            foreach (Cart ct in CartList)
            {
                Cart cart = new Cart
                {
                    BookId = ct.Book.Id,
                    Quantity = ct.Quantity,
                    UserId = id

                };

                _context.Cart.Add(cart);
                _context.SaveChanges();
            }

            return View(CartList);
        }

        [Authorize]
        public async Task<IActionResult> GetSavedBooks()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var thisUsersCart = await _context.Cart.Where(x => x.UserId == id).ToListAsync();
            foreach(Cart c in thisUsersCart)
            {
                c.Book = await bd.GetBook(c.BookId.Value);
            }
            
            return View(thisUsersCart);
        }

        [Authorize]
        public IActionResult CheckoutOrder()
        {
            string cartJson = HttpContext.Session.GetString("CartList");
            List<Cart> CartList = new List<Cart>();

            if (cartJson != null)
            {
                CartList = JsonSerializer.Deserialize<List<Cart>>(cartJson);
            }
            else if (cartJson == null)
            {
                string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                CartList = _context.Cart.Where(x => x.UserId == id).ToList();
                foreach (Cart c in CartList)
                {
                    c.Book = bd.GetBook(c.BookId.Value).Result;
                }
            }

            Checkout checkout = new Checkout();
            checkout.CheckoutItem = new List<CheckoutItem>();
            foreach (var c in CartList)
            {
                checkout.CheckoutItem.Add(new CheckoutItem { Book = c.Book, Quantity = c.Quantity, Subtotal = c.Quantity * c.Book.Price });

            }
            checkout.Subtotal = checkout.CheckoutItem.Sum(x => x.Subtotal);
            checkout.SalesTax = Math.Round((decimal)(checkout.Subtotal * (decimal)0.06), 2);
            checkout.Total = Math.Round((decimal)(checkout.Subtotal + checkout.SalesTax), 2);
            return View(checkout);
        }

        public IActionResult UpdateQuantity(int cartId, int quantity)
        {
            string cartJson = HttpContext.Session.GetString("CartList");
            Cart item = _context.Cart.Find(cartId);
            if(item != null)
            {
                item.Quantity = quantity;
                _context.Entry(item).State = EntityState.Modified;
                _context.Update(item);
                _context.SaveChanges();
            }
            List<Cart> CartList = new List<Cart>();
            if(cartJson != null)
            {
                CartList = JsonSerializer.Deserialize<List<Cart>>(cartJson);
            }
            Cart cartToUpdate = CartList.Single(x => x.CartId == cartId);
            cartToUpdate.Quantity = quantity;
            string cartListJson = JsonSerializer.Serialize(CartList);
            HttpContext.Session.SetString("CartList", cartListJson);

            return RedirectToAction("Cart", item);
        }
    }
}
