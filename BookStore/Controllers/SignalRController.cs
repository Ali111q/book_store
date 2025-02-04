using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

using BookStore.Controllers;
using BookStore.Extensions;

namespace BookStore.Controllers
{
    [Route("api/notifications")]
    [ApiController]
    public class NotificationController : BaseController
    {
        private readonly IHubContext<SignalRNotificationHub> _hubContext;
        private static readonly ConcurrentDictionary<Guid, string> _connectedUsers = new(); // Store userId -> connectionId

        public NotificationController(IHubContext<SignalRNotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendNotification(Guid userId, [FromBody] object message)
        {
            if (_connectedUsers.TryGetValue(userId, out var connectionId))
            {
                await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveNotification", message);
                return Ok("Notification sent.");
            }

            return NotFound("User not connected.");
        }

        [HttpGet("connected-users")]
        public IActionResult GetConnectedUsers()
        {
            return Ok(_connectedUsers.Keys);
        }

        public static void AddUser(Guid userId, string connectionId)
        {
            _connectedUsers[userId] = connectionId;
        }

        public static void RemoveUser(Guid userId)
        {
            _connectedUsers.TryRemove(userId, out _);
        }
    }
}