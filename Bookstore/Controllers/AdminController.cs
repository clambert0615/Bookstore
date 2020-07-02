using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bookstore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.AspNetCore.Identity;

namespace Bookstore.Controllers
{


    public class AdminController : Controller
    {
        private readonly BookstoreDAL bd = new BookstoreDAL();
        private readonly BookstoreContext _context;

        [Authorize(Roles = "Adminstrator")]
        public IActionResult Index()
        {

            return View();
        }
        [HttpPost]
        public IActionResult GetOption(int option)
        {
            if (option == 1)
            {
                return RedirectToAction("ListBooks");
            }
            else if (option == 2)
            {
                return RedirectToAction("AddBook");
            }
            else if (option == 3)
            {
                return RedirectToAction("GetBooktoUpdate");
            }
            else
            {
                return RedirectToAction("GetBooktoDelete");
            }


        }
        public async Task<IActionResult> ListBooks()
        {
            List<Books> bookList = await bd.GetBooks();
            return View(bookList);
        }



        [Authorize(Roles = "Adminstrator")]
        public IActionResult AddBook()
        {
            return View();
        }
        [Authorize(Roles = "Adminstrator")]
        public async Task<IActionResult> AddNewBook(Books newBook)
        {
            await bd.AddBook(newBook);
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Administrator")]
        public IActionResult GetBooktoUpdate()
        {
            return View();
        }
        [Authorize(Roles = "Adminstrator")]
        [HttpGet]
        public async Task<IActionResult> UpdateBook(int id)
        {
            Books foundBook = await bd.GetBook(id);
            return View(foundBook);
        }
        [HttpPost]
        [Authorize(Roles = "Adminstrator")]
        public async Task<IActionResult> UpdateBook(Books updatedBook)
        {
            Books oldBook = await bd.GetBook(updatedBook.Id);
            oldBook.Title = updatedBook.Title;
            oldBook.Author = updatedBook.Author;
            oldBook.PublishDate = updatedBook.PublishDate;
            oldBook.Pages = updatedBook.Pages;
            oldBook.Genre = updatedBook.Genre;
            oldBook.Price = updatedBook.Price;
            oldBook.Image = updatedBook.Image;
            oldBook.Description = updatedBook.Description;
            bd.UpdateBook(oldBook);
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Administrator")]
        public IActionResult GetBooktoDelete()
        {
            return View();
        }
        [Authorize(Roles = "Adminstrator")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            bd.DeleteBook(id);
            return RedirectToAction("Index");
        }
    }
}
