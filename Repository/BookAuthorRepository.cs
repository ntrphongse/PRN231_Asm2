using BusinessObject;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class BookAuthorRepository : IBookAuthorRepository
    {
        public async Task<BookAuthor> AddBookAuthorAsync(BookAuthor newBookAuthor)
            => await BookAuthorDAO.Instance.AddBookAuthorAsync(newBookAuthor);

        public async Task DeleteBookAuthorAsync(int authorId, int bookId) => await BookAuthorDAO.Instance.DeleteBookAuthorAsync(authorId, bookId);

        public async Task<BookAuthor> GetBookAuthorAsync(int authorId, int bookId) => await BookAuthorDAO.Instance.GetBookAuthorAsync(authorId, bookId);

        public async Task<IEnumerable<BookAuthor>> GetBookAuthorsAsync() => await BookAuthorDAO.Instance.GetBookAuthorsAsync();

        public async Task<BookAuthor> UpdateBookAuthorAsync(BookAuthor updatedBookAuthor)
            => await BookAuthorDAO.Instance.UpdateBookAuthorAsync(updatedBookAuthor);
    }
}
