using System.Collections.Generic;
using System.Threading.Tasks;
using BookLibraryApi.Models;

namespace BookLibraryApi.Repository
{
    public interface IBookRepository
    {
        public Task<Book> AddBook(Book book);
        public Task<Book> GetBook(int id);
        public Task<IEnumerable<Book>> GetAllBooks();
        public Task<Book> DeleteBook(int id);
        public Task<Book> UpdateBook(int id, Book book);
        public Task<int> SaveChangesAsync();
    }
}