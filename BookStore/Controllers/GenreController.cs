using BookStore.Data.Dto.Genre;
using BookStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    public class GenreController : BaseController
    {
        private readonly IGenreService _service;

        public GenreController(IGenreService service)
        {
            _service = service;
        }

        // GET: api/genres
        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery] GenreFilter filter)
        {
            var result = await _service.GetAll(filter);
            return Ok(result, filter.Page, filter.PageSize);  // Return the result with a 200 OK response
        }

        // GET: api/genres/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            var result = await _service.GetById(id);

            return Ok(result);  // Return the genre with a 200 OK response
        }

        // POST: api/genres
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] GenreForm form)
        {
            var result = await _service.Add(form);

            return Ok(result);  // Return 201 Created
        }

        // PUT: api/genres/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] GenreUpdate form)
        {
            var result = await _service.Update(form, id);

            return Ok(result);  // Return the updated genre with a 200 OK response
        }

        // DELETE: api/genres/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _service.Delete(id);

            return Ok(result);  // Return 204 No Content for successful deletion
        }
    }
}