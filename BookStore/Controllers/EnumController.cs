using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers;


public class EnumController : BaseController{

    [HttpGet("GetEnums")]
    public IActionResult GetEnums()
    {
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
        var id = Id;
        var enums = Assembly.GetAssembly(typeof(EnumController))
            ?.GetTypes()
            .Where(type => type.IsEnum)
            .Select(type => new
            {
                type.Name,
                Values = Enum.GetValues(type)
                    .Cast<Enum>()
                    .Select(value => new { Name = value.ToString() })
            })
            .ToList();

        return Ok(new { Enums = enums });
    }
}