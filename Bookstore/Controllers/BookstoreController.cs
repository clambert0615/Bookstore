using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bookstore.Models;
using Microsoft.AspNetCore.Mvc;

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
    }
}
