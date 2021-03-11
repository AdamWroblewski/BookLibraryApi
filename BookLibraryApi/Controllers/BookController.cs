using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BookLibraryApi.Models;
using BookLibraryApi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly SignInManager<IdentityUser> _signInManager;

        public BookController(IBookRepository bookRepository, SignInManager<IdentityUser> signInManager)
        {
            _bookRepository = bookRepository;
            _signInManager = signInManager;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            return await _bookRepository.GetBook(id, "");
        }

        private async Task<IdentityUser> GetCurrentUser()
        {
            var claims = User.Identity as ClaimsIdentity;
            var userName = claims.FindFirst(ClaimTypes.Name)?.Value;
            return await _signInManager.UserManager.FindByNameAsync(userName);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetAllBooks()
        {
            var identityUser = await GetCurrentUser();
            var books = await _bookRepository.GetAllBooks(identityUser.Id);
            return books.ToList();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBook(int id)
        {
            var identityUser = await GetCurrentUser();
            await _bookRepository.DeleteBook(id, identityUser.Id);
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
            book.User = await GetCurrentUser();
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
            var identityUser = await GetCurrentUser();
            var record = await _bookRepository.UpdateBook(id, book, identityUser.Id);
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