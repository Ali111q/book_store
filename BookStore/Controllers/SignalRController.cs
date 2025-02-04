using BookStore.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using BookStore.Services;

[Route("api/[controller]")]
[ApiController]
public class NotificationController : BaseController
{
    private readonly IUserConnectionManager _connectionManager;
    private readonly IHubContext<SignalRNotificationHub> _hubContext;

    public NotificationController(IUserConnectionManager connectionManager, IHubContext<SignalRNotificationHub> hubContext)
    {
        _connectionManager = connectionManager;
        _hubContext = hubContext;
    }

    [HttpGet("connected-users")]
    public IActionResult GetConnectedUsers()
    {
        var users = _connectionManager.GetConnectedUsers();
        return Ok(users);
    }

    [HttpPost("send-notification/{userId}")]
    public async Task<IActionResult> SendNotification(Guid userId, [FromBody] object message)
    {
        var connectionId = _connectionManager.GetConnectionId(userId);
        if (connectionId != null)
        {
            await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveNotification", message);
            return Ok("Notification sent");
        }
        return NotFound("User is not connected");
    }
}