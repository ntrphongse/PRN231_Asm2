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
    public class RolesController : Controller
    {
        //private readonly eStoreContext _context;

        public RolesController()
        {
        }

        // GET: Roles
        public async Task<IActionResult> Index()
        {
            try
            {
                HttpResponseMessage response = await eBookStoreClientUtils.ApiRequest(
                    eBookStoreHttpMethod.GET,
                    eBookStoreClientConfiguration.DefaultOdataUrl + "/Roles");

                if (response.IsSuccessStatusCode)
                {
                    //string strData = await response.Content.ReadAsStringAsync();

                    //dynamic responseObject = await response.Content.ReadAsAsync<dynamic>();

                    //dynamic temp = JObject.Parse(strData);
                    //ODataResponse<IEnumerable<Author>> temp = await response.Content.ReadAsAsync<ODataResponse<IEnumerable<Author>>>();

                    //IEnumerable<Author> authors =
                    //    temp.value;
                    IEnumerable<Role> roles = await response.ReadODataAsAsync<IEnumerable<Role>>();
                    return View(roles);
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
                ViewData["Roles"] = ex.Message;
                return View();
            }
        }

        // GET: Roles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    throw new Exception("Role is not specified!");
                }
                HttpResponseMessage response = await eBookStoreClientUtils.ApiRequest(
                    eBookStoreHttpMethod.GET,
                    eBookStoreClientConfiguration.DefaultOdataUrl + $"/Roles({id})");

                if (response.IsSuccessStatusCode)
                {
                    Role role = await response.Content.ReadAsAsync<Role>();

                    return View(role);
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
                ViewData["Roles"] = ex.Message;
                return View();
            }
        }

        // GET: Roles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoleDesc")] Role role)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    HttpResponseMessage response = await eBookStoreClientUtils.ApiRequest(
                    eBookStoreHttpMethod.POST,
                    eBookStoreClientConfiguration.DefaultOdataUrl + "/Roles", role);

                    if (response.IsSuccessStatusCode)
                    {
                        Role createdRole = await response.Content.ReadAsAsync<Role>();
                        if (createdRole == null)
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
                    ViewData["Roles"] = ex.Message;
                    return View(role);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }

        // GET: Roles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    throw new Exception("Role is not specified!");
                }
                HttpResponseMessage response = await eBookStoreClientUtils.ApiRequest(
                    eBookStoreHttpMethod.GET,
                    eBookStoreClientConfiguration.DefaultOdataUrl + $"/Roles({id})");

                if (response.IsSuccessStatusCode)
                {
                    Role role = await response.Content.ReadAsAsync<Role>();

                    return View(role);
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
                ViewData["Roles"] = ex.Message;
                return View();
            }
        }

        // POST: Roles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RoleId,RoleDesc")] Role role)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    HttpResponseMessage response = await eBookStoreClientUtils.ApiRequest(
                    eBookStoreHttpMethod.PUT,
                    eBookStoreClientConfiguration.DefaultOdataUrl + $"/Roles({id})", role);

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
                    ViewData["Roles"] = ex.Message;
                    return View(role);
                }
            }
            return View(role);
        }

        // GET: Roles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    throw new Exception("Role is not specified!");
                }
                HttpResponseMessage response = await eBookStoreClientUtils.ApiRequest(
                    eBookStoreHttpMethod.GET,
                    eBookStoreClientConfiguration.DefaultOdataUrl + $"/Roles({id})");

                if (response.IsSuccessStatusCode)
                {
                    Role role = await response.Content.ReadAsAsync<Role>();

                    return View(role);
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
                ViewData["Roles"] = ex.Message;
                return View();
            }
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                HttpResponseMessage response = await eBookStoreClientUtils.ApiRequest(
                eBookStoreHttpMethod.DELETE,
                eBookStoreClientConfiguration.DefaultOdataUrl + $"/Roles({id})");

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
                ViewData["Roles"] = ex.Message;
                return View();
            }
        }

    }
}
