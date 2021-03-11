using System.Collections.Generic;
using System.Threading.Tasks;
using BookLibraryApi.Models;

namespace BookLibraryApi.Repository
{
    public interface IBookRepository
    {
        public Task<Book> AddBook(Book book);
        public Task<Book> GetBook(int bookId, string userId);
        public Task<IEnumerable<Book>> GetAllBooks(string userId);
        public Task<Book> DeleteBook(int bookId, string userId);
        public Task<Book> UpdateBook(int bookId, Book book, string userId);
        public Task<int> SaveChangesAsync();
    }
}