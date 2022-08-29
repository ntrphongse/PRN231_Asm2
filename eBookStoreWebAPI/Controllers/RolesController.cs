using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using Repository;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.Authorization;

namespace eBookStoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ODataController
    {
        private readonly IRoleRepository roleRepository;

        public RolesController(IRoleRepository roleRepository)
        {
            this.roleRepository = roleRepository;
        }

        // GET: api/Roles
        //[HttpGet]
        [Authorize(Roles = "ADMIN,USER")]
        [EnableQuery]
        [ProducesResponseType(typeof(IEnumerable<Role>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                return StatusCode(200, await roleRepository.GetRolesAsync());
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

        // GET: api/Roles/5
        //[HttpGet("{id}")]
        [Authorize(Roles = "ADMIN")]
        [EnableQuery]
        [ProducesResponseType(typeof(Role), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetRole([FromODataUri] int key)
        {
            try
            {
                Role role = await roleRepository.GetRoleAsync(key);
                if (role == null)
                {
                    return StatusCode(404, "Role is not existed!!");
                }
                return StatusCode(200, role);
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

        // PUT: api/Roles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        [EnableQuery]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutRole([FromODataUri] int key, Role role)
        {
            if (key != role.RoleId)
            {
                return StatusCode(400, "ID is not the same!!");
            }

            try
            {
                await roleRepository.UpdateRoleAsync(role);
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

        // POST: api/Roles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        [Authorize(Roles = "ADMIN")]
        [EnableQuery]
        [ProducesResponseType(typeof(Role), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostRole(Role role)
        {
            try
            {
                Role createdRole = await roleRepository.AddRoleAsync(role);
                return StatusCode(201, createdRole);
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

        // DELETE: api/Roles/5
        //[HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        [EnableQuery]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteRole([FromODataUri] int key)
        {
            try
            {
                await roleRepository.DeleteRoleAsync(key);
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
