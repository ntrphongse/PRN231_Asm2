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
    public class BooksController : Controller
    {
        //private readonly eStoreContext _context;

        public BooksController()
        {
        }

        // GET: Books
        public async Task<IActionResult> Index([FromQuery] string searchString,
            [FromQuery] decimal? startPrice, [FromQuery] decimal? endPrice)
        {
            try
            {
                string fetchUrl = eBookStoreClientConfiguration.DefaultOdataUrl + "/Books?";

                if ((!string.IsNullOrEmpty(searchString) && (startPrice.HasValue || endPrice.HasValue))
                    || (string.IsNullOrEmpty(searchString) && (!startPrice.HasValue || !endPrice.HasValue))
                    && !(string.IsNullOrEmpty(searchString) && !startPrice.HasValue && !endPrice.HasValue))
                {
                    ViewData["SearchError"] = "Invalid search or filter!!";
                    return View();
                }
                if (!string.IsNullOrEmpty(searchString) && !startPrice.HasValue && !endPrice.HasValue)
                {
                    ViewData["search"] = searchString;
                    fetchUrl += $"$filter=contains(tolower(Title), tolower('{searchString}'))" +
                        $"&$expand=" + nameof(Book.Publisher);

                }
                else if (startPrice.HasValue && endPrice.HasValue)
                {
                    if (startPrice.Value > endPrice.Value)
                    {
                        decimal? temp = startPrice;
                        startPrice = endPrice;
                        endPrice = temp;
                    }
                    ViewData["StartPrice"] = startPrice.Value;
                    ViewData["EndPrice"] = endPrice.Value;
                    fetchUrl += $"$filter=Price ge {startPrice.Value} and Price le {endPrice.Value}" +
                        $"&$expand=" + nameof(Book.Publisher); ;
                }
                else
                {
                    fetchUrl += $"$expand=" + nameof(Book.Publisher);
                }
                HttpResponseMessage response = await eBookStoreClientUtils.ApiRequest(
                    eBookStoreHttpMethod.GET, fetchUrl
                    );

                if (response.IsSuccessStatusCode)
                {
                    //string strData = await response.Content.ReadAsStringAsync();

                    //dynamic responseObject = await response.Content.ReadAsAsync<dynamic>();

                    //dynamic temp = JObject.Parse(strData);
                    //ODataResponse<IEnumerable<Book>> temp = await response.Content.ReadAsAsync<ODataResponse<IEnumerable<Book>>>();

                    //IEnumerable<Book> books =
                    //    temp.value;
                    IEnumerable<Book> books = await response.ReadODataAsAsync<IEnumerable<Book>>();
                    return View(books);
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

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    throw new Exception("Book is not specified!");
                }
                HttpResponseMessage response = await eBookStoreClientUtils.ApiRequest(
                    eBookStoreHttpMethod.GET,
                    eBookStoreClientConfiguration.DefaultOdataUrl 
                    + $"/Books({id})?" +
                    $"$expand={nameof(Book.Publisher)}");

                if (response.IsSuccessStatusCode)
                {
                    Book book = await response.Content.ReadAsAsync<Book>();

                    return View(book);
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

        // GET: Books/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                IEnumerable<Publisher> publishers = await GetPublishersAsync();
                ViewData["PublisherId"] = new SelectList(publishers, nameof(Publisher.PublisherId), nameof(Publisher.PublisherName));
                return View();
            } catch (Exception ex)
            {
                ViewData["Books"] = ex.Message;
                return View();
            }
            //ViewData["PublisherId"] = new SelectList(_context.Publishers, "PublisherId", "City");
            //return View();
        }

        private async Task<IEnumerable<Publisher>> GetPublishersAsync()
        {
                HttpResponseMessage response = await eBookStoreClientUtils.ApiRequest(
                    eBookStoreHttpMethod.GET,
                    eBookStoreClientConfiguration.DefaultOdataUrl + "/Publishers");

                if (response.IsSuccessStatusCode)
                {
                    IEnumerable<Publisher> publishers = await response.ReadODataAsAsync<IEnumerable<Publisher>>();

                return publishers;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                throw new Exception("Failed to get publishers! Please log out and log in again...");
                }
                else
                {
                    throw new Exception(await response.ReadODataAsAsync<string>());
                }

        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Type,PublisherId,Price,Advance,Royalty,YtdSales,Notes,PublishedDate")] Book book)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    IEnumerable<Publisher> publishers = await GetPublishersAsync();
                    ViewData["PublisherId"] = new SelectList(publishers, nameof(Publisher.PublisherId), nameof(Publisher.PublisherName));

                    HttpResponseMessage response = await eBookStoreClientUtils.ApiRequest(
                    eBookStoreHttpMethod.POST,
                    eBookStoreClientConfiguration.DefaultOdataUrl + "/Books", book);

                    if (response.IsSuccessStatusCode)
                    {
                        Book createdBook = await response.Content.ReadAsAsync<Book>();
                        if (createdBook == null)
                        {
                            throw new Exception("Failed to create book!! Please check again...");
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
                    ViewData["Books"] = ex.Message;
                    return View(book);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    throw new Exception("Book is not specified!");
                }
                IEnumerable<Publisher> publishers = await GetPublishersAsync();
                ViewData["PublisherId"] = new SelectList(publishers, nameof(Publisher.PublisherId), nameof(Publisher.PublisherName));
                HttpResponseMessage response = await eBookStoreClientUtils.ApiRequest(
                    eBookStoreHttpMethod.GET,
                    eBookStoreClientConfiguration.DefaultOdataUrl
                    + $"/Books({id})?" +
                    $"$expand={nameof(Book.Publisher)}");

                if (response.IsSuccessStatusCode)
                {
                    Book book = await response.Content.ReadAsAsync<Book>();

                    return View(book);
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

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookId,Title,Type,PublisherId,Price,Advance,Royalty,YtdSales,Notes,PublishedDate")] Book book)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    IEnumerable<Publisher> publishers = await GetPublishersAsync();
                    ViewData["PublisherId"] = new SelectList(publishers, nameof(Publisher.PublisherId), nameof(Publisher.PublisherName));
                    HttpResponseMessage response = await eBookStoreClientUtils.ApiRequest(
                    eBookStoreHttpMethod.PUT,
                    eBookStoreClientConfiguration.DefaultOdataUrl + $"/Books({id})", book);

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
                    ViewData["Books"] = ex.Message;
                    return View(book);
                }
            }
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    throw new Exception("Book is not specified!");
                }

                HttpResponseMessage response = await eBookStoreClientUtils.ApiRequest(
                    eBookStoreHttpMethod.GET,
                    eBookStoreClientConfiguration.DefaultOdataUrl 
                    + $"/Books({id})?" +
                    $"$expand={nameof(Book.Publisher)}");

                if (response.IsSuccessStatusCode)
                {
                    Book book = await response.Content.ReadAsAsync<Book>();

                    return View(book);
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

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                HttpResponseMessage response = await eBookStoreClientUtils.ApiRequest(
                eBookStoreHttpMethod.DELETE,
                eBookStoreClientConfiguration.DefaultOdataUrl + $"/Books({id})");

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
                ViewData["Books"] = ex.Message;
                return View();
            }
        }

    }
}
