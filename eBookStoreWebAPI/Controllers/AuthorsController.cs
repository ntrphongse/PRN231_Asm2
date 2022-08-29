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
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.AspNetCore.Authorization;

namespace eBookStoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "ADMIN")]
    public class AuthorsController : ODataController
    {
        private readonly IAuthorRepository authorRepository;

        public AuthorsController(IAuthorRepository authorRepository)
        {
            this.authorRepository = authorRepository;
        }

        // GET: api/Authors
        //[HttpGet]
        [EnableQuery]
        [ProducesResponseType(typeof(IEnumerable<Author>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Get()
        {
            try
            {
                return StatusCode(200, await authorRepository.GetAuthorsAsync());
            } catch (ApplicationException ae)
            {
                return StatusCode(400, ae.Message);
            } catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //GET: api/Authors/5
        //[HttpGet("{id}")]
        [EnableQuery]
        [ProducesResponseType(typeof(Author), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Get([FromODataUri] int key)
        {
            try
            {
                Author author = await authorRepository.GetAuthorAsync(key);
                if (author == null)
                {
                    return StatusCode(404, "Author is not existed!!");
                }
                return StatusCode(200, author);
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

        // PUT: api/Authors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        [EnableQuery]
        //[HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Put([FromODataUri] int key, Author author)
        {
            if (key != author.AuthorId)
            {
                return StatusCode(400, "ID is not the same!!");
            }

            try
            {
                await authorRepository.UpdateAuthorAsync(author);
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

        // POST: api/Authors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        [EnableQuery]
        [ProducesResponseType(typeof(Author), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Post(Author author)
        {
            try
            {
                Author createdAuthor = await authorRepository.AddAuthorAsync(author);
                return StatusCode(201, createdAuthor);
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

        // DELETE: api/Authors/5
        //[HttpDelete("{id}")]
        [EnableQuery]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Delete([FromODataUri] int key)
        {
            try
            {
                await authorRepository.DeleteAuthorAsync(key);
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
