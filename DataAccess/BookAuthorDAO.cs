using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class BookAuthorDAO
    {
        private static BookAuthorDAO instance = null;
        private static readonly object instanceLock = new object();

        private BookAuthorDAO()
        {

        }

        public static BookAuthorDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new BookAuthorDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<IEnumerable<BookAuthor>> GetBookAuthorsAsync()
        {
            var db = new eStoreContext();
            return await db.BookAuthors
                .Include(ba => ba.Book)
                .Include(ba => ba.Author)
                .ToListAsync();
        }

        public async Task<BookAuthor> GetBookAuthorAsync(int authorId, int bookId)
        {
            var db = new eStoreContext();
            return await db.BookAuthors
                .Include(ba => ba.Book)
                .Include(ba => ba.Author)
                .SingleOrDefaultAsync(ba => ba.AuthorId == authorId
                                    && ba.BookId == bookId);
        }

        public async Task<BookAuthor> AddBookAuthorAsync(BookAuthor newBookAuthor)
        {
            CheckBookAuthor(newBookAuthor);
            if (await GetBookAuthorAsync(newBookAuthor.AuthorId, newBookAuthor.BookId) != null)
            {
                throw new ApplicationException("BookAuthor is existed!!");
            }
            var db = new eStoreContext();
            newBookAuthor.AuthorOrder = await GetNextAuthorOrder(newBookAuthor.BookId);
            await db.BookAuthors.AddAsync(newBookAuthor);
            await db.SaveChangesAsync();

            return newBookAuthor;
        }

        private async Task<bool> IsExistedAuthorOrder(int bookId, int authorOrder)
        {
            var db = new eStoreContext();
            BookAuthor bookAuthor = await db.BookAuthors
                .Where(ba => ba.BookId == bookId && ba.AuthorOrder == authorOrder)
                .FirstOrDefaultAsync();
            return bookAuthor != null;
        }

        private async Task<int> GetNextAuthorOrder(int bookId)
        {
            var db = new eStoreContext();
            IEnumerable<BookAuthor> bookAuthors = await db.BookAuthors
                .Where(ba => ba.BookId == bookId).ToListAsync();
            if (bookAuthors.Any())
            {
                return bookAuthors.Max(ba => ba.AuthorOrder) + 1;
            }
            return 1;
        }

        public async Task<BookAuthor> UpdateBookAuthorAsync(BookAuthor updatedBookAuthor)
        {
            BookAuthor bookAuthor = await GetBookAuthorAsync(updatedBookAuthor.AuthorId, updatedBookAuthor.BookId);
            if (bookAuthor == null)
            {
                throw new ApplicationException("BookAuthor does not exist!!");
            }
            CheckBookAuthor(updatedBookAuthor);
            if (bookAuthor.AuthorOrder != updatedBookAuthor.AuthorOrder &&
                await IsExistedAuthorOrder(updatedBookAuthor.BookId, updatedBookAuthor.AuthorOrder))
            {
                throw new ApplicationException("Author Order is existed!! Please check with the developer...");
            }
            var db = new eStoreContext();
            db.BookAuthors.Update(updatedBookAuthor);
            await db.SaveChangesAsync();
            return updatedBookAuthor;
        }

        public async Task DeleteBookAuthorAsync(int authorId, int bookId)
        {
            BookAuthor deletedAuthor = await GetBookAuthorAsync(authorId, bookId);
            if (deletedAuthor == null)
            {
                throw new ApplicationException("BookAuthor does not exist!!");
            }
            var db = new eStoreContext();
            db.BookAuthors.Remove(deletedAuthor);
            await db.SaveChangesAsync();
        }

        private async void CheckBookAuthor(BookAuthor bookAuthor)
        {
            var db = new eStoreContext();
            if (await db.Authors.FindAsync(bookAuthor.AuthorId) == null)
            {
                throw new ApplicationException("Author does not exist!!");
            }
            if (await db.Books.FindAsync(bookAuthor.BookId) == null)
            {
                throw new ApplicationException("Book does not exist!!");
            }
        }
    }
}
