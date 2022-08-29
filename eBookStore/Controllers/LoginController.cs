using BusinessObject;
using DTO;
using eBookStoreLibrary;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eBookStore.Controllers
{
    public class LoginController : Controller
    {
        // GET: LoginController
        [AllowAnonymous]
        public IActionResult Index([FromQuery] string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                if (eBookStoreClientUtils.IsAdmin(User))
                {
                    return RedirectToAction("Index", "Books");
                }
                else
                {
                    return RedirectToAction("Details", "Users", 
                        new { id = User.Claims.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.NameIdentifier)).Value.ToString()});
                }
            }
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm, Bind("Email", "Password")] UserLoginDTO userLoginInfo,
                                                [FromForm, Bind("ReturnUrl")] string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                if (eBookStoreClientUtils.IsAdmin(User))
                {
                    return RedirectToAction("Index", "Books");
                }
                else
                {
                    return RedirectToAction("Details", "Users",
                        new { id = User.Claims.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.NameIdentifier)).Value.ToString() });
                }
            }
            try
            {
                HttpResponseMessage response = await eBookStoreClientUtils.ApiRequest(
                eBookStoreHttpMethod.POST,
                eBookStoreClientConfiguration.DefaultApiUrl + "/Users/Login",
                userLoginInfo);

                if (response.IsSuccessStatusCode)
                {
                    AuthorizeUser loginUser = await response.Content.ReadAsAsync<AuthorizeUser>();
                    if (loginUser == null)
                    {
                        throw new Exception("Failed to login! Please check again...");
                    }
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, loginUser.EmailAddress),
                        new Claim(ClaimTypes.NameIdentifier, loginUser.UserId.ToString()),
                        new Claim(ClaimTypes.Role, loginUser.AuthorizeRole),
                        new Claim(ClaimTypes.Name, loginUser.AuthorizeRole.Equals("ADMIN") ? "Admin" : loginUser.FirstName + " " + loginUser.LastName)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var memberPrincipal = new ClaimsPrincipal(new[] { claimsIdentity });

                    await HttpContext.SignInAsync(memberPrincipal);

                    if (string.IsNullOrEmpty(returnUrl))
                    {
                        if (loginUser.AuthorizeRole.Equals("ADMIN"))
                        {
                            return RedirectToAction("Index", "Books");
                        }
                        else
                        {
                            return RedirectToAction("Details", "Users",
                                new { id = loginUser.UserId.ToString() });
                        }
                    }
                    return Redirect(returnUrl);

                }
                else
                {
                    throw new Exception(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                ViewData["Login"] = ex.Message;
                return View();
            }
        }

        [AllowAnonymous]
        public IActionResult AccessDenied([FromQuery] string returnUrl)
        {
            ViewData["returnUrl"] = returnUrl;
            return View();
        }
    }
}
