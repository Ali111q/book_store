using BookStore.Data.Dto.Ad;
using BookStore.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    public class AdController : BaseController
    {
        private readonly IAdService _service;

        public AdController(IAdService service)
        {
            _service = service;
        }

        
        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery] AdFilter filter)
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

        /// <summary>
        /// Adds a new advertisement.
        /// </summary>
        /// <remarks>
        /// Validation rules:
        /// - The `Type` must be a valid `AdType` enum value. 
        /// - If the `Type` is `RANDOM_ADD(3)`, the `AdLink` field is required.
        /// - If the `Type` is not `RANDOM_ADD(3)`, the `RefId` field is required.
        /// </remarks>
        [HttpPost]
        [Authorize(Roles = "Admin", AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult> Add([FromBody] AdForm form)
        {
            var result = await _service.Add(form);
            return Ok(result);  
        }

        
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin", AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult> Update(Guid id, [FromBody] AdUpdate form)
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
