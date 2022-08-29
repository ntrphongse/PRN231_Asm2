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
using System.Security.Claims;

namespace eBookStore.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        //private readonly eStoreContext _context;

        public UsersController()
        {
        }

        // GET: Users
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Index()
        {
            try
            {
                HttpResponseMessage response = await eBookStoreClientUtils.ApiRequest(
                    eBookStoreHttpMethod.GET,
                    eBookStoreClientConfiguration.DefaultOdataUrl
                    + "/Users?$expand="
                        + nameof(BusinessObject.User.Publisher) + ","
                        + nameof(BusinessObject.User.Role));

                if (response.IsSuccessStatusCode)
                {
                    IEnumerable<User> users = await response.ReadODataAsAsync<IEnumerable<User>>();
                    return View(users);
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
                ViewData["Users"] = ex.Message;
                return View();
            }
        }

        private async Task<IEnumerable<Role>> GetRolesAsync()
        {
            HttpResponseMessage response = await eBookStoreClientUtils.ApiRequest(
                    eBookStoreHttpMethod.GET,
                    eBookStoreClientConfiguration.DefaultOdataUrl + "/Roles");

            if (response.IsSuccessStatusCode)
            {
                IEnumerable<Role> roles = await response.ReadODataAsAsync<IEnumerable<Role>>();

                return roles;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new Exception("Failed to get roles! Please log out and log in again...");
            }
            else
            {
                throw new Exception(await response.ReadODataAsAsync<string>());
            }
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

        //GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    throw new Exception("User is not specified!");
                }
                string role = User.Claims.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.Role)).Value;
                if (role.Equals("USER"))
                {
                    int userId = int.Parse(User.Claims.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.NameIdentifier)).Value);
                    if (id != userId)
                    {
                        return RedirectToAction("AccessDenied", "Login");
                    }
                }
                HttpResponseMessage response = await eBookStoreClientUtils.ApiRequest(
                    eBookStoreHttpMethod.GET,
                    eBookStoreClientConfiguration.DefaultOdataUrl + $"/Users({id})?" +
                    $"$expand="
                        + nameof(BusinessObject.User.Publisher) + ","
                        + nameof(BusinessObject.User.Role));

                if (response.IsSuccessStatusCode)
                {
                    User user = await response.Content.ReadAsAsync<User>();

                    return View(user);
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
                ViewData["Users"] = ex.Message;
                return View();
            }
        }

        // GET: Users/Create
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Create()
        {
            try
            {
                ViewData["PublisherId"] = new SelectList(await GetPublishersAsync(), nameof(Publisher.PublisherId), nameof(Publisher.PublisherName));
                ViewData["RoleId"] = new SelectList(await GetRolesAsync(), nameof(Role.RoleId), nameof(Role.RoleDesc));
                return View();
            }
            catch (Exception ex)
            {
                ViewData["Users"] = ex.Message;
                return View();
            }
            //ViewData["PublisherId"] = new SelectList(_context.Publishers, "PublisherId", "City");
            //ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleDesc");
            //return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Create([Bind("EmailAddress,Password,Source,FirstName,MiddleName,LastName,HireDate,RoleId,PublisherId")] User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ViewData["PublisherId"] = new SelectList(await GetPublishersAsync(), nameof(Publisher.PublisherId), nameof(Publisher.PublisherName));
                    ViewData["RoleId"] = new SelectList(await GetRolesAsync(), nameof(Role.RoleId), nameof(Role.RoleDesc));

                    HttpResponseMessage response = await eBookStoreClientUtils.ApiRequest(
                    eBookStoreHttpMethod.POST,
                    eBookStoreClientConfiguration.DefaultOdataUrl + "/Users", user);

                    if (response.IsSuccessStatusCode)
                    {
                        User createdUser = await response.Content.ReadAsAsync<User>();
                        if (createdUser == null)
                        {
                            throw new Exception("Failed to create user!! Please check again...");
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
                    ViewData["Users"] = ex.Message;
                    return View(user);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    throw new Exception("User is not specified!");
                }
                string role = User.Claims.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.Role)).Value;
                if (role.Equals("USER"))
                {
                    int userId = int.Parse(User.Claims.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.NameIdentifier)).Value);
                    if (id != userId)
                    {
                        return RedirectToAction("AccessDenied", "Login");
                    }
                }
                ViewData["PublisherId"] = new SelectList(await GetPublishersAsync(), nameof(Publisher.PublisherId), nameof(Publisher.PublisherName));
                ViewData["RoleId"] = new SelectList(await GetRolesAsync(), nameof(Role.RoleId), nameof(Role.RoleDesc));
                HttpResponseMessage response = await eBookStoreClientUtils.ApiRequest(
                    eBookStoreHttpMethod.GET,
                    eBookStoreClientConfiguration.DefaultOdataUrl + $"/Users({id})?" +
                    $"$expand="
                        + nameof(BusinessObject.User.Publisher) + ","
                        + nameof(BusinessObject.User.Role));

                if (response.IsSuccessStatusCode)
                {
                    User user = await response.Content.ReadAsAsync<User>();

                    return View(user);
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
                ViewData["Users"] = ex.Message;
                return View();
            }
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,EmailAddress,Password,Source,FirstName,MiddleName,LastName,HireDate,RoleId,PublisherId")] User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string role = User.Claims.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.Role)).Value;
                    if (role.Equals("USER"))
                    {
                        int userId = int.Parse(User.Claims.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.NameIdentifier)).Value);
                        if (id != userId)
                        {
                            return RedirectToAction("AccessDenied", "Login");
                        }
                    }
                    ViewData["PublisherId"] = new SelectList(await GetPublishersAsync(), nameof(Publisher.PublisherId), nameof(Publisher.PublisherName));
                    ViewData["RoleId"] = new SelectList(await GetRolesAsync(), nameof(Role.RoleId), nameof(Role.RoleDesc));
                    HttpResponseMessage response = await eBookStoreClientUtils.ApiRequest(
                    eBookStoreHttpMethod.PUT,
                    eBookStoreClientConfiguration.DefaultOdataUrl + $"/Users({id})", user);

                    if (response.IsSuccessStatusCode)
                    {
                        if (role.Equals("USER"))
                        {
                            return RedirectToAction("Details", "Users",
                                new { id = id.ToString() });
                        }
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
                    ViewData["Users"] = ex.Message;
                    return View(user);
                }
            }
            return View(user);
        }

        // GET: Users/Delete/5
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    throw new Exception("User is not specified!");
                }
                HttpResponseMessage response = await eBookStoreClientUtils.ApiRequest(
                    eBookStoreHttpMethod.GET,
                    eBookStoreClientConfiguration.DefaultOdataUrl + $"/Users({id})?" +
                    $"$expand="
                        + nameof(BusinessObject.User.Publisher) + ","
                        + nameof(BusinessObject.User.Role));

                if (response.IsSuccessStatusCode)
                {
                    User user = await response.Content.ReadAsAsync<User>();

                    return View(user);
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
                ViewData["Users"] = ex.Message;
                return View();
            }
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                HttpResponseMessage response = await eBookStoreClientUtils.ApiRequest(
                eBookStoreHttpMethod.DELETE,
                eBookStoreClientConfiguration.DefaultOdataUrl + $"/Users({id})");

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
                ViewData["Users"] = ex.Message;
                return View();
            }
        }

    }
}
