using eBookStoreLibrary;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eBookStore.Controllers
{
    public class LogoutController : Controller
    {
        [Authorize]
        public async Task<IActionResult> Index()
        {
            await eBookStoreClientUtils.ApiRequest(
                eBookStoreHttpMethod.POST,
                eBookStoreClientConfiguration.DefaultOdataUrl + "/Users/logout");
            await HttpContext.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}
