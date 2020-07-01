using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoreAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookAPIController : ControllerBase
    {
        private readonly BookstoreAPIDbContext _context;

        public BookAPIController(BookstoreAPIDbContext context)
        {
            _context = context;
        }

        //Get book list with endpoint api/bookapi
        [HttpGet]
        public async Task<ActionResult<List<Books>>> GetBooks()
        {
            var books = await _context.Books.ToListAsync();
            return books;
        }

        //Get one book with endpoint api/bookapi/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Books>> GetBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            else
            {
                return book;
            }
        }
        //Post: api/bookapi
        [HttpPost]
        public async Task<ActionResult<Books>> AddBook(Books newBook)
        {
            if (ModelState.IsValid)
            {
                _context.Books.Add(newBook);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetBook), new { id = newBook.Id }, newBook);
            }

            else
            {
                return BadRequest();
            }
        }
        //Update api/bookapi/{id}
        [HttpPut]
        public async Task<ActionResult> UpdateBook(Books updatedBook)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                _context.Entry(updatedBook).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return NoContent();
            }
        }
        //Delete api/bookapi/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if(book == null)
            {
                return NotFound();
            }
            else
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
                return NoContent();
            }
        }
    }
}
