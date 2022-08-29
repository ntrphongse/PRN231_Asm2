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
    
    public class PublishersController : ODataController
    {
        private readonly IPublisherRepository publisherRepository;

        public PublishersController(IPublisherRepository publisherRepository)
        {
            this.publisherRepository = publisherRepository;
        }

        // GET: api/Publishers
        //[HttpGet]
        [Authorize(Roles = "ADMIN,USER")]
        [EnableQuery]
        [ProducesResponseType(typeof(IEnumerable<Publisher>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPublishers()
        {
            try
            {
                return StatusCode(200, await publisherRepository.GetPublishersAsync());
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

        // GET: api/Publishers/5
        //[HttpGet("{id}")]
        [EnableQuery]
        [Authorize(Roles = "ADMIN")]
        [ProducesResponseType(typeof(Publisher), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPublisher([FromODataUri] int key)
        {
            try
            {
                Publisher publisher = await publisherRepository.GetPublisherAsync(key);
                if (publisher == null)
                {
                    return StatusCode(404, "Publisher is not existed!!");
                }
                return StatusCode(200, publisher);
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

        // PUT: api/Publishers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        [EnableQuery]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutPublisher([FromODataUri] int key, Publisher publisher)
        {
            if (key != publisher.PublisherId)
            {
                return StatusCode(400, "ID is not the same!!");
            }

            try
            {
                await publisherRepository.UpdatePublisherAsync(publisher);
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

        // POST: api/Publishers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        [Authorize(Roles = "ADMIN")]
        [EnableQuery]
        [ProducesResponseType(typeof(Publisher), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostPublisher(Publisher publisher)
        {
            try
            {
                Publisher createdPublisher = await publisherRepository.AddPublisherAsync(publisher);
                return StatusCode(201, createdPublisher);
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

        // DELETE: api/Publishers/5
        //[HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        [EnableQuery]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeletePublisher([FromODataUri] int key)
        {
            try
            {
                await publisherRepository.DeletePublisherAsync(key);
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
