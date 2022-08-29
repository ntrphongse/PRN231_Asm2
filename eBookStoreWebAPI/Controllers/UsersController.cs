using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using Repository;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Formatter;
using DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace eBookStoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ODataController
    {
        private readonly IUserRepository userRepository;

        public UsersController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        // GET: api/Users
        //[HttpGet]
        [EnableQuery]
        [ProducesResponseType(typeof(IEnumerable<User>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                //IEnumerable<User> users = await userRepository.GetUsersAsync();
                return StatusCode(200, await userRepository.GetUsersAsync());
            }
            catch (ApplicationException ae)
            {
                return StatusCode(400, ae.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //[EnableQuery]
        //[ODataRoute("Users", RouteName = "Login")]
        [HttpPost("Login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthorizeUser), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostLogin(UserLoginDTO userLogin)
        {
            try
            {
                string memberRole = "USER";
                User loginUser = await userRepository.LoginAsync(userLogin.Email, userLogin.Password);

                if (loginUser == null)
                {
                    throw new ApplicationException("Failed to login! Please check the information again...");
                }
                
                User defaultUser = userRepository.GetDefaultUser();
                if (loginUser.EmailAddress.Equals(defaultUser.EmailAddress))
                {
                    memberRole = "ADMIN";
                }
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, loginUser.EmailAddress),
                    new Claim(ClaimTypes.NameIdentifier, loginUser.UserId.ToString()),
                    new Claim(ClaimTypes.Role, memberRole)
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = false,
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), authProperties);

                //loginUser.MemberId = 0;
                loginUser.Password = "";
                AuthorizeUser authorizeUser = new AuthorizeUser(loginUser);
                authorizeUser.AuthorizeRole = memberRole;

                return StatusCode(200, authorizeUser);
            } catch (ApplicationException ae)
            {
                return StatusCode(401, ae.Message);
            } catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //[EnableQuery]
        [HttpPost("Logout")]
        [Authorize]
        public async Task<IActionResult> PostLogout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return StatusCode(204);
        }

        // GET: api/Users/5
        //[HttpGet("{id}")]
        [EnableQuery]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetUser([FromODataUri] int key)
        {
            try
            {
                string role = User.Claims.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.Role)).Value;
                if (role.Equals("USER"))
                {
                    int id = int.Parse(User.Claims.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.NameIdentifier)).Value);
                    if (key != id)
                    {
                        return Unauthorized();
                    }
                }
                User user = await userRepository.GetUserAsync(key);
                if (user == null)
                {
                    return StatusCode(404, "User is not existed!!");
                }
                return StatusCode(200, user);
            }
            catch (ApplicationException ae)
            {
                return StatusCode(400, ae.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        [EnableQuery]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutUser([FromODataUri] int key, User user)
        {
            if (key != user.UserId)
            {
                return StatusCode(400, "ID is not the same!!");
            }

            try
            {
                string role = User.Claims.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.Role)).Value;
                if (role.Equals("USER"))
                {
                    int id = int.Parse(User.Claims.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.NameIdentifier)).Value);
                    if (key != id)
                    {
                        return Unauthorized();
                    }
                }
                await userRepository.UpdateUserAsync(user);
                return StatusCode(204, "Update successfully!");
            }
            catch (ApplicationException ae)
            {
                return StatusCode(400, ae.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        [EnableQuery]
        [ProducesResponseType(typeof(User), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostUser(User user)
        {
            try
            {
                User createdUser = await userRepository.AddUserAsync(user);
                return StatusCode(201, createdUser);
            }
            catch (ApplicationException ae)
            {
                return StatusCode(400, ae.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/Users/5
        //[HttpDelete("{id}")]
        [EnableQuery]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteUser([FromODataUri] int key)
        {
            try
            {
                await userRepository.DeleteUserAsync(key);
                return StatusCode(204, "Delete successfully!");
            }
            catch (ApplicationException ae)
            {
                return StatusCode(400, ae.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
