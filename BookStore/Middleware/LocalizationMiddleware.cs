public class LocalizationMiddleware
{
    private readonly RequestDelegate _next;

    public LocalizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var lang = context.Request.Headers["Accept-Language"].FirstOrDefault() ?? "en";
        context.Items["Lang"] = lang; // Store language in HttpContext

        await _next(context);
    }
}