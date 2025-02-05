using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using black_follow.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class AuditMiddleware
{
    #region Fields
    private readonly RequestDelegate _next;
    private readonly ILogger<AuditMiddleware> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    #endregion

    #region Constructor
    public AuditMiddleware(RequestDelegate next, ILogger<AuditMiddleware> logger, IServiceScopeFactory scopeFactory)
    {
        _next = next;
        _logger = logger;
        _scopeFactory = scopeFactory;
    }
    #endregion

    #region Middleware Invocation
    public async Task Invoke(HttpContext context)
    {
        var request = await FormatRequest(context.Request);
        var originalBodyStream = context.Response.Body;
        
        using (var responseBody = new MemoryStream())
        {
            context.Response.Body = responseBody;
            await _next(context);
            var response = await FormatResponse(context.Response);
            await responseBody.CopyToAsync(originalBodyStream);
            await SaveAuditLog(request, response, context);
        }
    }
    #endregion

    #region Request Formatting
    private async Task<string> FormatRequest(HttpRequest request)
    {
        request.EnableBuffering();

        if (request.ContentType?.StartsWith("multipart/form-data") == true)
        {
            request.Body.Position = 0;
            return $"{request.Method} {request.Path} {request.QueryString} - Body: [File Upload Omitted]";
        }

        var body = await new StreamReader(request.Body).ReadToEndAsync();
        request.Body.Position = 0;

        // Modify the request body if it contains any "password" keys
        body = MaskPasswordFields(body);

        return $"{request.Method} {request.Path} {request.QueryString} - Body: {body}";
    }

    private string MaskPasswordFields(string body)
    {
        // Assuming the body is JSON, this can be adapted if you're using form data or another format.
        try
        {
            var json = Newtonsoft.Json.Linq.JObject.Parse(body);
            foreach (var property in json.Properties())
            {
                if (property.Name.ToLower().Contains("password"))
                {
                    property.Value = "$$$$$$";
                }
            }
            return json.ToString();
        }
        catch (Exception)
        {
            // If not JSON, return the body as is (or you can implement other checks for different content types)
            return body;
        }
    }

    #endregion

    #region Response Formatting
    private async Task<string> FormatResponse(HttpResponse response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);
        var text = await new StreamReader(response.Body).ReadToEndAsync();
        response.Body.Seek(0, SeekOrigin.Begin);
        return $"Status: {response.StatusCode} - Body: {text}";
    }
    #endregion

    #region Save Audit Log
    private async Task SaveAuditLog(string request, string response, HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/api/Audit", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        var _userId = context.User.FindFirst("id")?.Value;
        
        using (var scope = _scopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
            var auditLog = new AuditLog
            {
                Request = request,
                Response = response,
                User = _userId ?? "",
            };

            dbContext.AuditLogs.Add(auditLog);
            await dbContext.SaveChangesAsync();
        }
    }
    #endregion
}