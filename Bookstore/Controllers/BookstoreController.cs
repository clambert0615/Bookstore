
using Bookstore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Permissions;
using System.Text.Json;
using System.Threading.Tasks;

namespace Bookstore.Controllers
{
    public class BookstoreController : Controller
    {
        private readonly BookstoreContext _context;
        private readonly BookstoreDAL bd = new BookstoreDAL();
        private readonly NYTimesDAL ny;

        public BookstoreController(BookstoreContext context, IConfiguration configuration)
        {
            _context = context;
            ny = new NYTimesDAL(configuration);
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

        [Authorize]
        public async Task<IActionResult> AddToCart(int id, int quantity)
        {
            List<Cart> CartList = new List<Cart>();


            Books foundBook = await bd.GetBook(id);

            Cart cartbook = new Cart(foundBook, quantity);

            //Add found book to the cart list and return user to the list of books
            string userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (_context.Cart.Where(x => (x.UserId == userid) && (x.BookId == foundBook.Id)).ToList().Count == 0)
            {
                if (quantity <= foundBook.Quantity)
                {

                    CartList.Add(cartbook);
                    userid = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    foreach (Cart ct in CartList)
                    {
                        Cart cart = new Cart
                        {
                            BookId = ct.Book.Id,
                            Quantity = ct.Quantity,
                            UserId = userid

                        };

                        _context.Cart.Add(cart);
                        _context.SaveChanges();
                    }



                    return RedirectToAction("Cart");
                }
            }

            return RedirectToAction("Cart");
        }

        public async Task<IActionResult> Cart()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var thisUsersCart = await _context.Cart.Where(x => x.UserId == id).ToListAsync();
            foreach (Cart c in thisUsersCart)
            {
                c.Book = await bd.GetBook(c.BookId.Value);
            }

            if (thisUsersCart == null)
            {
                return View("SearchIndex");
            }

            return View(thisUsersCart);
        }


        [Authorize]
        public IActionResult CheckoutOrder()
        {

            List<Cart> CartList = new List<Cart>();


            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            CartList = _context.Cart.Where(x => x.UserId == id).ToList();
            foreach (Cart c in CartList)
            {
                c.Book = bd.GetBook(c.BookId.Value).Result;

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

        [Authorize]
        public IActionResult UpdateQuantity(int cartId, int quantity)
        {
            // string cartJson = HttpContext.Session.GetString("CartList");
            Cart item = _context.Cart.Find(cartId);
            if (quantity == 0)
            {
                _context.Cart.Remove(item);
                _context.SaveChanges();
            }
            if (item != null && quantity >= 1)
            {
                item.Quantity = quantity;
                _context.Entry(item).State = EntityState.Modified;
                _context.Update(item);
                _context.SaveChanges();
            }

            return RedirectToAction("Cart", item);
        }
        [Authorize]
        public IActionResult Payment(Payment newPayment)
        {
            //if this is real, then I would have it go to a credit card processor
            string userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var list = _context.Cart.Where(x => x.UserId == userid).ToList();
            foreach (var c in list)
            {
                Books book = bd.GetBook(c.BookId.Value).Result;
                book.Quantity = book.Quantity - c.Quantity;
                bd.UpdateBook(book);
                _context.Cart.Remove(c);
                _context.SaveChanges();
            }


            return RedirectToAction("Confirmation");
        }
        public IActionResult Confirmation()
        {
            return View();
        }

        public IActionResult BestSellerList()
        {
            IEnumerable<Book> results = ny.getBestSeller().results.books.ToList();
            return View(results);
        }
    }
}
