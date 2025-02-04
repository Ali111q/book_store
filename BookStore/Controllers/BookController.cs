using BookStore.Data.Dto.Book;
using BookStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    public class BookController : BaseController
    {
        private readonly IBookService _service;

        public BookController(IBookService service)
        {
            _service = service;
        }

        // GET: api/books
        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery] BookFilter filter)
        {
            var result = await _service.GetAll(filter);
            return Ok(result, filter.Page, filter.PageSize);  // Return the result with a 200 OK response
        }

        // GET: api/books/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            var result = await _service.GetById(id);

         

            return Ok(result);  // Return the book with a 200 OK response
        }

        // POST: api/books
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] BookForm form)
        {
            var result = await _service.Add(form);

           

            return Ok( result);  // Return 201 Created
        }

        // PUT: api/books/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] BookUpdate form)
        {
            var result = await _service.Update(form, id);

         
            return Ok(result);  // Return the updated book with a 200 OK response
        }

        // DELETE: api/books/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _service.Delete(id);

           

            return Ok(result);  // Return 204 No Content for successful deletion
        }
    }
}
