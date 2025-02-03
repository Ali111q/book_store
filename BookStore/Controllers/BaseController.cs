using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers;
[ApiController]
[Route("api/[controller]")]
public abstract class BaseController: ControllerBase
{
    protected virtual string GetClaim(string claimName) {
        var claims = (User.Identity as ClaimsIdentity)?.Claims;
        var claim = claims?.FirstOrDefault(c =>
            string.Equals(c.Type, claimName, StringComparison.CurrentCultureIgnoreCase) &&
            !string.Equals(c.Type, "null", StringComparison.CurrentCultureIgnoreCase));
        var rr = claim?.Value!.Replace("\"", "");

        return rr ?? "";
    }
    protected Guid Id => Guid.TryParse(GetClaim("Id"), out var id) ? id : Guid.Empty;
    protected string Role => GetClaim("Role");

    protected string MethodType => HttpContext.Request.Method;
    protected ObjectResult Ok<T>((T obj, string? error) result) =>
        result.error != null
            ? base.BadRequest(new {Message = result.error})
            : base.Ok(result.obj);
}