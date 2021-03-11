using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLibraryApi.Models;
using BookLibraryApi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookLibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            return await _bookRepository.GetBook(id);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetAllBooks()
        {
            var books = await _bookRepository.GetAllBooks();
            return books.ToList();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBook(int id)
        {
            await _bookRepository.DeleteBook(id);
            try
            {
                await _bookRepository.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult> AddBook(Book book)
        {
            var record = await _bookRepository.AddBook(book);

            try
            {
                await _bookRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return CreatedAtAction("GetBook", new {id = book.Id}, record);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBook(int id, Book book)
        {
            var record = await _bookRepository.UpdateBook(id, book);
            if (record == null)
                return NotFound();

            try
            {
                await _bookRepository.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}