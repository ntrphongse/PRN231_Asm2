using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IBookAuthorRepository
    {
        Task<IEnumerable<BookAuthor>> GetBookAuthorsAsync();
        Task<BookAuthor> GetBookAuthorAsync(int authorId, int bookId);
        Task<BookAuthor> AddBookAuthorAsync(BookAuthor newBookAuthor);
        Task<BookAuthor> UpdateBookAuthorAsync(BookAuthor updatedBookAuthor);
        Task DeleteBookAuthorAsync(int authorId, int bookId);
    }
}
