using BusinessObject;
using eBookStoreLibrary;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class BookDAO
    {
        private static BookDAO instance = null;
        private static readonly object instanceLock = new object();

        private BookDAO()
        {

        }

        public static BookDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new BookDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            var db = new eStoreContext();
            return await db.Books
                .Include(book => book.Publisher)
                .ToListAsync();
        }

        public async Task<Book> GetBookAsync(int bookId)
        {
            var db = new eStoreContext();
            return await db.Books
                .Include(book => book.Publisher)
                .SingleOrDefaultAsync(book => book.BookId == bookId);
        }

        public async Task<Book> AddBookAsync(Book newBook)
        {
            await CheckBookAsync(newBook);
            var db = new eStoreContext();
            await db.Books.AddAsync(newBook);
            await db.SaveChangesAsync();

            return newBook;
        }

        public async Task<Book> UpdateBookAsync(Book updatedBook)
        {
            if (await GetBookAsync(updatedBook.BookId) == null)
            {
                throw new ApplicationException("Book does not exist!!");
            }
            await CheckBookAsync(updatedBook);
            var db = new eStoreContext();
            db.Books.Update(updatedBook);
            await db.SaveChangesAsync();
            return updatedBook;
        }

        public async Task DeleteBookAsync(int bookId)
        {
            Book deletedBook = await GetBookAsync(bookId);
            if (deletedBook == null)
            {
                throw new ApplicationException("Book does not exist!!");
            }
            var db = new eStoreContext();
            db.Books.Remove(deletedBook);
            await db.SaveChangesAsync();
        }

        private async Task CheckBookAsync(Book book)
        {
            book.Title.StringValidate(
                allowEmpty: false,
                emptyErrorMessage: "Book Title is required!!",
                minLength: 2,
                minLengthErrorMessage: "Book Title must have at least 2 characters!!",
                maxLength: 255,
                maxLengthErrorMessage: "Book Title is limited to 255 characters!!"
                );

            book.Type.StringValidate(
                allowEmpty: false,
                emptyErrorMessage: "Book Type is required!!",
                minLength: 2,
                minLengthErrorMessage: "Book Type must have at least 2 characters!!",
                maxLength: 50,
                maxLengthErrorMessage: "Book Type is limited to 50 characters!!"
                );


            book.Price.DecimalValidate(
                minimum: 0,
                minErrorMessage: "Book Price must be a positive number!",
                maximum: decimal.MaxValue,
                maxErrorMessage: "Book Price must be less than " + decimal.MaxValue + "!!"
                );

            book.YtdSales.IntegerValidate(
                minimum: 0,
                minErrorMessage: "Book Year to Date sales must be a positive integer!",
                maximum: int.MaxValue,
                maxErrorMessage: $"Book Year to Date sales must be less than {int.MaxValue}!!"
                );

            book.Advance.StringValidate(
                allowEmpty: true,
                emptyErrorMessage: "",
                minLength: 2,
                minLengthErrorMessage: "Book Advance must have at least 2 characters!!",
                maxLength: 255,
                maxLengthErrorMessage: "Book Advance is limited to 255 characters!!"
                );

            book.Royalty.StringValidate(
                allowEmpty: true,
                emptyErrorMessage: "",
                minLength: 1,
                minLengthErrorMessage: "Book Royalty must have at least 1 characters!!",
                maxLength: 50,
                maxLengthErrorMessage: "Book Royalty is limited to 50 characters!!"
                );

            book.Notes.StringValidate(
                allowEmpty: true,
                emptyErrorMessage: "",
                minLength: 2,
                minLengthErrorMessage: "Book Notes must have at least 2 characters!!",
                maxLength: 255,
                maxLengthErrorMessage: "Book Royalty is limited to 255 characters!!"
                );

            if (book.PublishedDate > DateTime.Now)
            {
                throw new ApplicationException("Book Published Date must not be after today!!");
            }

            var db = new eStoreContext();
            if (await db.Publishers.FindAsync(book.PublisherId) == null)
            {
                throw new ApplicationException("Book Publisher does not exist!");
            }
        }
    }
}
