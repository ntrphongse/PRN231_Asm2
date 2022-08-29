using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetBooksAsync();
        Task<Book> GetBookAsync(int bookId);
        Task<Book> AddBookAsync(Book newBook);
        Task<Book> UpdateBookAsync(Book updatedBook);
        Task DeleteBookAsync(int bookId);
        
    }
}
