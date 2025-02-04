using System.Security.Claims;
using BookStore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

[Authorize(AuthenticationSchemes = "Bearer")]

public class SignalRNotificationHub : Hub
{
    private readonly IUserConnectionManager _connectionManager;

    public SignalRNotificationHub(IUserConnectionManager connectionManager)
    {
        _connectionManager = connectionManager;
    }


    public override async Task OnConnectedAsync()
    {
        var userIdClaim =Context?.User?.FindFirst("id")?.Value;
        if (Guid.TryParse(userIdClaim, out var userId))
        {
            _connectionManager.AddUser(userId, Context.ConnectionId);
            await Groups.AddToGroupAsync(Context.ConnectionId, userId.ToString());
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (Guid.TryParse(Context?.GetHttpContext()?.Request.Query["userId"], out var userId))
        {
            _connectionManager.RemoveUser(userId);
        }

        await base.OnDisconnectedAsync(exception);
    }
    public async Task SendNotificationToUser(Guid userId, object message)
    {
        var connectionId = _connectionManager.GetConnectionId(userId);
        if (!string.IsNullOrEmpty(connectionId))
        {
            await Clients.Client(connectionId).SendAsync("ReceiveNotification", message);
        }
    }

}