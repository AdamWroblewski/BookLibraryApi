using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLibraryApi.Contexts;
using BookLibraryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookLibraryApi.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly BookContext _context;

        public BookRepository(BookContext context)
        {
            _context = context;
        }

        public async Task<Book> AddBook(Book book)
        {
            if (book != null)
                await _context.AddAsync(book);

            return book;
        }

        public async Task<Book> GetBook(int bookId, string userId)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == bookId && b.User.Id == userId);
            return book;
        }

        public async Task<IEnumerable<Book>> GetAllBooks(string userId)
        {
            return await _context.Books.Where(b => b.User.Id == userId)
                .ToListAsync();
        }

        public async Task<Book> DeleteBook(int id, string userId)
        {
            var bookToDelete = await _context.Books.FirstOrDefaultAsync(b => b.Id == id && b.User.Id == userId);
            if (bookToDelete == null) return null;

            _context.Books.Remove(bookToDelete);
            return bookToDelete;
        }

        public async Task<Book> UpdateBook(int id, Book book, string userId)
        {
            if (id != book.Id || book.User.Id != userId)
                return null;

            var entity = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
            entity.Author = book.Author;
            entity.Title = book.Title;
            entity.ReleaseYear = book.ReleaseYear;
            entity.NumberOfPages = book.NumberOfPages;
            _context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}