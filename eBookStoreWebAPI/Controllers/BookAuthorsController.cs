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
using Microsoft.AspNetCore.Authorization;

namespace eBookStoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "ADMIN")]
    public class BookAuthorsController : ODataController
    {
        private readonly IBookAuthorRepository bookAuthorRepository;

        public BookAuthorsController(IBookAuthorRepository bookAuthorRepository)
        {
            this.bookAuthorRepository = bookAuthorRepository;
        }

        // GET: api/BookAuthors
        //[HttpGet]
        [EnableQuery]
        [ProducesResponseType(typeof(IEnumerable<BookAuthor>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetBookAuthors()
        {
            try
            {
                return StatusCode(200, await bookAuthorRepository.GetBookAuthorsAsync());
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

        //GET: api/BookAuthors/5
        //[HttpGet("{id}")]
        [EnableQuery]
        [ProducesResponseType(typeof(BookAuthor), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetBookAuthor([FromODataUri] int keyAuthorId, [FromODataUri] int keyBookId)
        {
            try
            {
                BookAuthor bookAuthor = await bookAuthorRepository.GetBookAuthorAsync(keyAuthorId, keyBookId);
                if (bookAuthor == null)
                {
                    return StatusCode(404, "BookAuthor is not existed!!");
                }
                return StatusCode(200, bookAuthor);
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

        // PUT: api/BookAuthors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{keyBookId}/{keyAuthorId}")]
        [EnableQuery]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutBookAuthor([FromODataUri] int keyAuthorId, [FromODataUri] int keyBookId, [FromBody] BookAuthor bookAuthor)
        {
            if (keyBookId != bookAuthor.BookId || keyAuthorId != bookAuthor.AuthorId)  
            {
                return StatusCode(400, "ID is not the same!!");
            }

            try
            {
                await bookAuthorRepository.UpdateBookAuthorAsync(bookAuthor);
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

        // POST: api/BookAuthors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        [EnableQuery]
        [ProducesResponseType(typeof(BookAuthor), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostBookAuthor(BookAuthor bookAuthor)
        {
            try
            {
                BookAuthor createdBookAuthor = await bookAuthorRepository.AddBookAuthorAsync(bookAuthor);
                return StatusCode(201, createdBookAuthor);
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

        // DELETE: api/BookAuthors/5
        //[HttpDelete("{keyBookId}/{keyAuthorId}")]
        [EnableQuery]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteBookAuthor([FromODataUri] int keyAuthorId, [FromODataUri] int keyBookId)
        {
            try
            {
                await bookAuthorRepository.DeleteBookAuthorAsync(keyAuthorId, keyBookId);
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
