using BookStore.Data.Dto.Book;
using BookStore.Services;
using Microsoft.AspNetCore.Authorization;
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

        
        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery] BookFilter filter)
        {
            var result = await _service.GetAll(filter);
            return Ok(result, filter.Page, filter.PageSize); 
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            var result = await _service.GetById(id);

         

            return Ok(result); 
        }

        
        [HttpPost]
        [Authorize(Roles = "Admin", AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult> Add([FromBody] BookForm form)
        {
            var result = await _service.Add(form);

           

            return Ok( result); 
        }

        
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin", AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult> Update(Guid id, [FromBody] BookUpdate form)
        {
            var result = await _service.Update(form, id);

         
            return Ok(result);  
        }

        
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin", AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _service.Delete(id);

           

            return Ok(result);  
        }
    }
}
