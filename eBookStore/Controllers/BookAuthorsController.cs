using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using System.Net.Http;
using eBookStoreLibrary;
using Microsoft.AspNetCore.Authorization;

namespace eBookStore.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class BookAuthorsController : Controller
    {
        //private readonly eStoreContext _context;

        public BookAuthorsController()
        {
        }

        // GET: BookAuthors
        public async Task<IActionResult> Index()
        {
            //var eStoreContext = _context.BookAuthors.Include(b => b.Author).Include(b => b.Book);
            //return View(await eStoreContext.ToListAsync());
            try
            {
                HttpResponseMessage response = await eBookStoreClientUtils.ApiRequest(
                    eBookStoreHttpMethod.GET,
                    eBookStoreClientConfiguration.DefaultOdataUrl 
                    + "/BookAuthors?$expand=" 
                        + nameof(BookAuthor.Author) + ","
                        + nameof(BookAuthor.Book) + "&" +
                        $"$orderby={nameof(BookAuthor.BookId)} asc," +
                                    $"{nameof(BookAuthor.AuthorOrder)} asc");

                if (response.IsSuccessStatusCode)
                {
                    //string strData = await response.Content.ReadAsStringAsync();

                    //dynamic responseObject = await response.Content.ReadAsAsync<dynamic>();

                    //dynamic temp = JObject.Parse(strData);
                    //ODataResponse<IEnumerable<Book>> temp = await response.Content.ReadAsAsync<ODataResponse<IEnumerable<Book>>>();

                    //IEnumerable<Book> books =
                    //    temp.value;
                    IEnumerable<BookAuthor> bookAuthors = await response.ReadODataAsAsync<IEnumerable<BookAuthor>>();
                    return View(bookAuthors);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("AccessDenied", "Login");
                }
                else
                {
                    throw new Exception(await response.ReadODataAsAsync<string>());
                }
            }
            catch (Exception ex)
            {
                ViewData["BookAuthors"] = ex.Message;
                return View();
            }
        }

        private async Task<BookAuthor> GetBookAuthorAsync(int authorId, int bookId)
        {
            HttpResponseMessage response = await eBookStoreClientUtils.ApiRequest(
                    eBookStoreHttpMethod.GET,
                    eBookStoreClientConfiguration.DefaultOdataUrl + $"/BookAuthors" +
                    $"(AuthorId={authorId},BookId={bookId})?" +
                    $"$expand={nameof(BookAuthor.Book)},{nameof(BookAuthor.Author)}");

            if (response.IsSuccessStatusCode)
            {
                BookAuthor bookAuthor = await response.Content.ReadAsAsync<BookAuthor>();

                return bookAuthor;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new Exception("Failed to get book authors! Please log out and log in again...");
            }
            else
            {
                throw new Exception(await response.ReadODataAsAsync<string>());
            }

        }

        private async Task<IEnumerable<Book>> GetBooksAsync()
        {
            HttpResponseMessage response = await eBookStoreClientUtils.ApiRequest(
                    eBookStoreHttpMethod.GET,
                    eBookStoreClientConfiguration.DefaultOdataUrl + "/Books");

            if (response.IsSuccessStatusCode)
            {
                IEnumerable<Book> books = await response.ReadODataAsAsync<IEnumerable<Book>>();

                return books;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new Exception("Failed to get books! Please log out and log in again...");
            }
            else
            {
                throw new Exception(await response.ReadODataAsAsync<string>());
            }
        }

        private async Task<IEnumerable<Author>> GetAuthorsAsync()
        {
            HttpResponseMessage response = await eBookStoreClientUtils.ApiRequest(
                    eBookStoreHttpMethod.GET,
                    eBookStoreClientConfiguration.DefaultOdataUrl + "/Authors");

            if (response.IsSuccessStatusCode)
            {
                IEnumerable<Author> authors = await response.ReadODataAsAsync<IEnumerable<Author>>();

                return authors;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new Exception("Failed to get authors! Please log out and log in again...");
            }
            else
            {
                throw new Exception(await response.ReadODataAsAsync<string>());
            }
        }

        // GET: BookAuthors/Details/5
        public async Task<IActionResult> Details(int? authorId, int? bookId)
        {
            try
            {
                if (authorId == null)
                {
                    throw new Exception("Author is not specified!");
                }

                if (bookId == null)
                {
                    throw new Exception("Book is not specified!");
                }

                IEnumerable<Book> books = await GetBooksAsync();
                IEnumerable<Author> authors = await GetAuthorsAsync();
                ViewData["AuthorId"] = new SelectList(authors, nameof(Author.AuthorId), nameof(Author.LastName));
                ViewData["BookId"] = new SelectList(books, nameof(Book.BookId), nameof(Book.Title));

                HttpResponseMessage response = await eBookStoreClientUtils.ApiRequest(
                    eBookStoreHttpMethod.GET,
                    eBookStoreClientConfiguration.DefaultOdataUrl + $"/BookAuthors" +
                    $"(AuthorId={authorId.Value},BookId={bookId.Value})?" +
                    $"$expand={nameof(BookAuthor.Book)},{nameof(BookAuthor.Author)}");

                if (response.IsSuccessStatusCode)
                {
                    BookAuthor bookAuthor = await response.Content.ReadAsAsync<BookAuthor>();

                    return View(bookAuthor);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("AccessDenied", "Login");
                }
                else
                {
                    throw new Exception(await response.ReadODataAsAsync<string>());
                }
            }
            catch (Exception ex)
            {
                ViewData["BookAuthors"] = ex.Message;
                return View();
            }
        }

        // GET: BookAuthors/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                IEnumerable<Book> books = await GetBooksAsync();
                IEnumerable<Author> authors = await GetAuthorsAsync();
                ViewData["AuthorId"] = new SelectList(authors, nameof(Author.AuthorId), nameof(Author.LastName));
                ViewData["BookId"] = new SelectList(books, nameof(Book.BookId), nameof(Book.Title));
                return View();
            }
            catch (Exception ex)
            {
                ViewData["BookAuthors"] = ex.Message;
                return View();
            }
            //ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "EmailAddress");
            //ViewData["BookId"] = new SelectList(_context.Books, "BookId", "Title");
            //return View();
        }

        // POST: BookAuthors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AuthorId,BookId,RoyalityPercentage")] BookAuthor bookAuthor)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    IEnumerable<Book> books = await GetBooksAsync();
                    IEnumerable<Author> authors = await GetAuthorsAsync();
                    ViewData["AuthorId"] = new SelectList(authors, nameof(Author.AuthorId), nameof(Author.LastName));
                    ViewData["BookId"] = new SelectList(books, nameof(Book.BookId), nameof(Book.Title));

                    HttpResponseMessage response = await eBookStoreClientUtils.ApiRequest(
                    eBookStoreHttpMethod.POST,
                    eBookStoreClientConfiguration.DefaultOdataUrl + "/BookAuthors", bookAuthor);

                    if (response.IsSuccessStatusCode)
                    {
                        BookAuthor createdBookAuthor = await response.Content.ReadAsAsync<BookAuthor>();
                        if (createdBookAuthor == null)
                        {
                            throw new Exception("Failed to create book author!! Please check again...");
                        }
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        return RedirectToAction("AccessDenied", "Login");
                    }
                    else
                    {
                        throw new Exception(await response.ReadODataAsAsync<string>());
                    }
                }
                catch (Exception ex)
                {
                    ViewData["BookAuthors"] = ex.Message;
                    return View(bookAuthor);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(bookAuthor);
        }

        // GET: BookAuthors/Edit/5/1
        public async Task<IActionResult> Edit(int? authorId, int? bookId)
        {
            try
            {
                if (authorId == null)
                {
                    throw new Exception("Author is not specified!");
                }

                if (bookId == null)
                {
                    throw new Exception("Book is not specified!");
                }

                IEnumerable<Book> books = await GetBooksAsync();
                IEnumerable<Author> authors = await GetAuthorsAsync();
                ViewData["AuthorId"] = new SelectList(authors, nameof(Author.AuthorId), nameof(Author.LastName));
                ViewData["BookId"] = new SelectList(books, nameof(Book.BookId), nameof(Book.Title));

                HttpResponseMessage response = await eBookStoreClientUtils.ApiRequest(
                    eBookStoreHttpMethod.GET,
                    eBookStoreClientConfiguration.DefaultOdataUrl + $"/BookAuthors" +
                    $"(AuthorId={authorId.Value},BookId={bookId.Value})?" +
                    $"$expand={nameof(BookAuthor.Book)},{nameof(BookAuthor.Author)}");

                if (response.IsSuccessStatusCode)
                {
                    BookAuthor bookAuthor = await response.Content.ReadAsAsync<BookAuthor>();

                    return View(bookAuthor);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("AccessDenied", "Login");
                }
                else
                {
                    throw new Exception(await response.ReadODataAsAsync<string>());
                }
            }
            catch (Exception ex)
            {
                ViewData["BookAuthors"] = ex.Message;
                return View();
            }
        }

        // POST: BookAuthors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? authorId, int? bookId, [Bind("AuthorId,BookId,AuthorOrder,RoyalityPercentage,Book,Author")] BookAuthor bookAuthor)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    IEnumerable<Book> books = await GetBooksAsync();
                    IEnumerable<Author> authors = await GetAuthorsAsync();
                    ViewData["AuthorId"] = new SelectList(authors, nameof(Author.AuthorId), nameof(Author.LastName));
                    ViewData["BookId"] = new SelectList(books, nameof(Book.BookId), nameof(Book.Title));
                    
                    HttpResponseMessage response = await eBookStoreClientUtils.ApiRequest(
                    eBookStoreHttpMethod.PUT,
                    eBookStoreClientConfiguration.DefaultOdataUrl + $"/BookAuthors" +
                    $"(AuthorId={authorId.Value},BookId={bookId.Value})", bookAuthor);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        return RedirectToAction("AccessDenied", "Login");
                    }
                    else
                    {
                        throw new Exception(await response.ReadODataAsAsync<string>());
                    }
                }
                catch (Exception ex)
                {
                    ViewData["BookAuthors"] = ex.Message;
                    bookAuthor = await GetBookAuthorAsync(bookAuthor.AuthorId, bookAuthor.BookId);
                    return View(bookAuthor);
                }
            }
            bookAuthor = await GetBookAuthorAsync(bookAuthor.AuthorId, bookAuthor.BookId);
            return View(bookAuthor);
        }

        // GET: BookAuthors/Delete/5
        public async Task<IActionResult> Delete(int? authorId, int? bookId)
        {
            try
            {
                if (authorId == null)
                {
                    throw new Exception("Author is not specified!");
                }

                if (bookId == null)
                {
                    throw new Exception("Book is not specified!");
                }

                IEnumerable<Book> books = await GetBooksAsync();
                IEnumerable<Author> authors = await GetAuthorsAsync();
                ViewData["AuthorId"] = new SelectList(authors, nameof(Author.AuthorId), nameof(Author.LastName));
                ViewData["BookId"] = new SelectList(books, nameof(Book.BookId), nameof(Book.Title));

                HttpResponseMessage response = await eBookStoreClientUtils.ApiRequest(
                    eBookStoreHttpMethod.GET,
                    eBookStoreClientConfiguration.DefaultOdataUrl + $"/BookAuthors" +
                    $"(AuthorId={authorId.Value},BookId={bookId.Value})?" +
                    $"$expand={nameof(BookAuthor.Book)},{nameof(BookAuthor.Author)}");

                if (response.IsSuccessStatusCode)
                {
                    BookAuthor bookAuthor = await response.Content.ReadAsAsync<BookAuthor>();

                    return View(bookAuthor);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("AccessDenied", "Login");
                }
                else
                {
                    throw new Exception(await response.ReadODataAsAsync<string>());
                }
            }
            catch (Exception ex)
            {
                ViewData["Books"] = ex.Message;
                return View();
            }
        }

        // POST: BookAuthors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? authorId, int? bookId)
        {
            try
            {

                HttpResponseMessage response = await eBookStoreClientUtils.ApiRequest(
                eBookStoreHttpMethod.DELETE,
                eBookStoreClientConfiguration.DefaultOdataUrl + $"/BookAuthors" +
                    $"(AuthorId={authorId.Value},BookId={bookId.Value})");

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("AccessDenied", "Login");
                }
                else
                {
                    throw new Exception(await response.ReadODataAsAsync<string>());
                }
            }
            catch (Exception ex)
            {
                ViewData["BookAuthors"] = ex.Message;
                return View();
            }
        }
    }
}
