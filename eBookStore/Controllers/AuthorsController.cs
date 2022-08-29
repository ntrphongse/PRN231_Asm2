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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using eBookStore.Models;
using Microsoft.AspNetCore.Authorization;

namespace eBookStore.Controllers
{

    [Authorize(Roles = "ADMIN")]
    public class AuthorsController : Controller
    {
        //private readonly eStoreContext _context;

        public AuthorsController()
        {
            
        }

        // GET: Authors
        public async Task<IActionResult> Index()
        {
            try
            {
                HttpResponseMessage response = await eBookStoreClientUtils.ApiRequest(
                    eBookStoreHttpMethod.GET,
                    eBookStoreClientConfiguration.DefaultOdataUrl + "/Authors");

                if (response.IsSuccessStatusCode)
                {
                    //string strData = await response.Content.ReadAsStringAsync();

                    //dynamic responseObject = await response.Content.ReadAsAsync<dynamic>();

                    //dynamic temp = JObject.Parse(strData);
                    //ODataResponse<IEnumerable<Author>> temp = await response.Content.ReadAsAsync<ODataResponse<IEnumerable<Author>>>();

                    //IEnumerable<Author> authors =
                    //    temp.value;
                    IEnumerable<Author> authors = await response.ReadODataAsAsync<IEnumerable<Author>>();
                    return View(authors);
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
                ViewData["Authors"] = ex.Message;
                return View();
            }
        }

        //GET: Authors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    throw new Exception("Author is not specified!");
                }
                HttpResponseMessage response = await eBookStoreClientUtils.ApiRequest(
                    eBookStoreHttpMethod.GET,
                    eBookStoreClientConfiguration.DefaultOdataUrl + $"/Authors({id})");

                if (response.IsSuccessStatusCode)
                {
                    Author author = await response.Content.ReadAsAsync<Author>();

                    return View(author);
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
                ViewData["Authors"] = ex.Message;
                return View();
            }
        }

        // GET: Authors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Authors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LastName,FirstName,Phone,Address,City,State,Zip,EmailAddress")] Author author)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    HttpResponseMessage response = await eBookStoreClientUtils.ApiRequest(
                    eBookStoreHttpMethod.POST,
                    eBookStoreClientConfiguration.DefaultOdataUrl + "/Authors", author);

                    if (response.IsSuccessStatusCode)
                    {
                        Author createdAuthor = await response.Content.ReadAsAsync<Author>();
                        if (createdAuthor == null)
                        {
                            throw new Exception("Failed to create author!! Please check again...");
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
                    ViewData["Authors"] = ex.Message;
                    return View(author);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        // GET: Authors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    throw new Exception("Author is not specified!");
                }
                HttpResponseMessage response = await eBookStoreClientUtils.ApiRequest(
                    eBookStoreHttpMethod.GET,
                    eBookStoreClientConfiguration.DefaultOdataUrl + $"/Authors({id})");

                if (response.IsSuccessStatusCode)
                {
                    Author author = await response.Content.ReadAsAsync<Author>();

                    return View(author);
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
                ViewData["Authors"] = ex.Message;
                return View();
            }
        }

        // POST: Authors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AuthorId,LastName,FirstName,Phone,Address,City,State,Zip,EmailAddress")] Author author)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    HttpResponseMessage response = await eBookStoreClientUtils.ApiRequest(
                    eBookStoreHttpMethod.PUT,
                    eBookStoreClientConfiguration.DefaultOdataUrl + $"/Authors({id})", author);

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
                    ViewData["Authors"] = ex.Message;
                    return View(author);
                }
            }
            return View(author);
        }

        // GET: Authors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    throw new Exception("Author is not specified!");
                }
                HttpResponseMessage response = await eBookStoreClientUtils.ApiRequest(
                    eBookStoreHttpMethod.GET,
                    eBookStoreClientConfiguration.DefaultOdataUrl + $"/Authors({id})");

                if (response.IsSuccessStatusCode)
                {
                    Author author = await response.Content.ReadAsAsync<Author>();

                    return View(author);
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
                ViewData["Authors"] = ex.Message;
                return View();
            }
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                HttpResponseMessage response = await eBookStoreClientUtils.ApiRequest(
                eBookStoreHttpMethod.DELETE,
                eBookStoreClientConfiguration.DefaultOdataUrl + $"/Authors({id})");

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
                ViewData["Authors"] = ex.Message;
                return View();
            }
        }

    }
}
