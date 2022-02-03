using BookStoreAPI.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreAPI.Controllers
{
    [Route("api/")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public BookController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        [HttpGet("books")]
        public async Task<ActionResult<List<Book>>> Get()
        {
            return Ok(await _dataContext.Books.ToListAsync());
        }

        [HttpGet("book/{Id}")]
        
        public async Task<ActionResult<Book>> Get(int Id)
        {
            var book = await _dataContext.Books.FindAsync(Id);
            if (book == null)
            {
                return BadRequest("Book Not Found!");
            }
            return Ok(book);
        }

        [HttpPost("book/add")]
        public async Task<ActionResult<List<Book>>> AddBook(Book book)
        {
            _dataContext.Books.Add(book);
            await _dataContext.SaveChangesAsync();
            return Ok(await _dataContext.Books.ToListAsync());
        }

        [HttpPut("book/update/{Id}")]
        public async Task<ActionResult<List<Book>>> UpdateBook(int Id, Book request)
        {
            if (Id != request.Id)
            {
                return BadRequest("Book id mismatch");
            }

            var book = await _dataContext.Books.FindAsync(request.Id);
            if (book == null)
            {
                return BadRequest("Book Not Found!");
            }

            book.Title = request.Title;
            book.Description = request.Description;

            await _dataContext.SaveChangesAsync();

            return Ok(book);
        }

        [HttpDelete("book/delete/{Id}")]
        public async Task<ActionResult<List<Book>>> DeleteBook(int Id)
        {
            var book = await _dataContext.Books.FindAsync(Id);
            if (book == null)
            {
                return BadRequest("Book Not Found!");
            }

            _dataContext.Books.Remove(book);
            await _dataContext.SaveChangesAsync();

            return Ok(await _dataContext.Books.ToListAsync());
        }
    }
}
