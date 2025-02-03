using BookStore.Data.Dto.Author;
using BookStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    public class AuthorController : BaseController
    {
        private readonly AuthorService _service;

        public AuthorController(AuthorService service)
        {
            _service = service;
        }

        // GET: api/authors
        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery] AuthorFilter filter)
        {
            var result = await _service.GetAll(filter);
            return Ok(result);  // Return the result with a 200 OK response
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
        public async Task<ActionResult> Add([FromBody] AuthorForm form)
        {
            var result = await _service.Add(form);
            if (result.data == null)
            {
                return BadRequest(result.message);  // Return 400 if there was an issue with adding
            }
            return CreatedAtAction(nameof(GetById), new { id = result.data?.Id }, result);  // Return 201 Created
        }

        // PUT: api/authors/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] AuthorUpdateForm form)
        {
            if (id != form.Id)
            {
                return BadRequest("Author ID mismatch.");
            }

            var result = await _service.Update(form);
           
            return Ok(result);  // Return the updated author with a 200 OK response
        }

        // DELETE: api/authors/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _service.Delete(id);
        
            return Ok(result);  // Return 204 No Content for successful deletion
        }
    }
}
