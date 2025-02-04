using BookStore.Data.Dto.Author;
using BookStore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    public class AuthorController : BaseController
    {
        private readonly IAuthorService _service;

        public AuthorController(IAuthorService service)
        {
            _service = service;
        }

        // GET: api/authors
        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery] AuthorFilter filter)
        {
            var result = await _service.GetAll(filter);
            return Ok<AuthorDto>(result, filter.Page, filter.PageSize);  // Return the result with a 200 OK response
        }

        // GET: api/authors/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            var result = await _service.GetById(id);
           
            return Ok(result);  // Return the author with a 200 OK response
        }

        // POST: api/authors
        [HttpPost]
        [Authorize(Roles = "Admin", AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult> Add([FromBody] AuthorForm form)
        {
            var result = await _service.Add(form);
          
            return Ok(result);  // Return 201 Created
        }

        // PUT: api/authors/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin", AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult> Update(Guid id, [FromBody] AuthorUpdate form)
        {
 

            var result = await _service.Update(form, id);
           
            return Ok(result);  // Return the updated author with a 200 OK response
        }

        // DELETE: api/authors/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin", AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _service.Delete(id);
        
            return Ok(result);  // Return 204 No Content for successful deletion
        }
    }
}
