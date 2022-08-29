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
    public class PublishersController : Controller
    {
        //private readonly eStoreContext _context;

        public PublishersController()
        {
        }

        // GET: Publishers
        public async Task<IActionResult> Index()
        {
            try
            {
                HttpResponseMessage response = await eBookStoreClientUtils.ApiRequest(
                    eBookStoreHttpMethod.GET,
                    eBookStoreClientConfiguration.DefaultOdataUrl + "/Publishers");

                if (response.IsSuccessStatusCode)
                {
                    IEnumerable<Publisher> publishers = await response.ReadODataAsAsync<IEnumerable<Publisher>>();
                    return View(publishers);
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
                ViewData["Publishers"] = ex.Message;
                return View();
            }
        }

        // GET: Publishers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    throw new Exception("Publisher is not specified!");
                }
                HttpResponseMessage response = await eBookStoreClientUtils.ApiRequest(
                    eBookStoreHttpMethod.GET,
                    eBookStoreClientConfiguration.DefaultOdataUrl + $"/Publishers({id})");

                if (response.IsSuccessStatusCode)
                {
                    Publisher publisher = await response.Content.ReadAsAsync<Publisher>();

                    return View(publisher);
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
                ViewData["Publishers"] = ex.Message;
                return View();
            }
        }

        // GET: Publishers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Publishers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PublisherName,City,State,Country")] Publisher publisher)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    HttpResponseMessage response = await eBookStoreClientUtils.ApiRequest(
                    eBookStoreHttpMethod.POST,
                    eBookStoreClientConfiguration.DefaultOdataUrl + "/Publishers", publisher);

                    if (response.IsSuccessStatusCode)
                    {
                        Publisher createdPublisher = await response.Content.ReadAsAsync<Publisher>();
                        if (createdPublisher == null)
                        {
                            throw new Exception("Failed to create publisher!! Please check again...");
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
                    ViewData["Publishers"] = ex.Message;
                    return View(publisher);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(publisher);
        }

        // GET: Publishers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    throw new Exception("Publisher is not specified!");
                }
                HttpResponseMessage response = await eBookStoreClientUtils.ApiRequest(
                    eBookStoreHttpMethod.GET,
                    eBookStoreClientConfiguration.DefaultOdataUrl + $"/Publishers({id})");

                if (response.IsSuccessStatusCode)
                {
                    Publisher publisher = await response.Content.ReadAsAsync<Publisher>();

                    return View(publisher);
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
                ViewData["Publishers"] = ex.Message;
                return View();
            }
        }

        // POST: Publishers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PublisherId,PublisherName,City,State,Country")] Publisher publisher)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    HttpResponseMessage response = await eBookStoreClientUtils.ApiRequest(
                    eBookStoreHttpMethod.PUT,
                    eBookStoreClientConfiguration.DefaultOdataUrl + $"/Publishers({id})", publisher);

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
                    ViewData["Publishers"] = ex.Message;
                    return View(publisher);
                }
            }
            return View(publisher);
        }

        // GET: Publishers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    throw new Exception("Publisher is not specified!");
                }
                HttpResponseMessage response = await eBookStoreClientUtils.ApiRequest(
                    eBookStoreHttpMethod.GET,
                    eBookStoreClientConfiguration.DefaultOdataUrl + $"/Publishers({id})");

                if (response.IsSuccessStatusCode)
                {
                    Publisher publisher = await response.Content.ReadAsAsync<Publisher>();

                    return View(publisher);
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
                ViewData["Publishers"] = ex.Message;
                return View();
            }
        }

        // POST: Publishers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                HttpResponseMessage response = await eBookStoreClientUtils.ApiRequest(
                eBookStoreHttpMethod.DELETE,
                eBookStoreClientConfiguration.DefaultOdataUrl + $"/Publishers({id})");

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
                ViewData["Publishers"] = ex.Message;
                return View();
            }
        }
    }
}
