using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace BookStore.Extensions
{
    public interface ISignalRNotificationHub
    {
        Task SendNotificationToUser(Guid userId, object message);
        Task<List<Guid>> GetConnectedUsers();
    }

    public class SignalRNotificationHub : Hub<ISignalRNotificationHub>
    {
        private static readonly ConcurrentDictionary<Guid, string> _connectedUsers = new(); // userId -> connectionId mapping

        public override async Task OnConnectedAsync()
        {
            try
            {
                if (Guid.TryParse(Context?.GetHttpContext()?.Request.Query["userId"], out var userId))
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, userId.ToString());
                    _connectedUsers[userId] = Context.ConnectionId;
                }
            }
            catch (Exception ex)
            {
                // Log exception if needed
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            try
            {
                if (Guid.TryParse(Context?.GetHttpContext()?.Request.Query["userId"], out var userId))
                {
                    _connectedUsers.TryRemove(userId, out _);
                }
            }
            catch (Exception ex)
            {
                // Log exception if needed
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendNotificationToUser(Guid userId, object message)
        {
            if (_connectedUsers.TryGetValue(userId, out var connectionId))
            {
                // await Clients.Client(connectionId).SendAsync("ReceiveNotification", message);
            }
        }

        public async Task<List<Guid>> GetConnectedUsers()
        {
            return await Task.FromResult(_connectedUsers.Keys.ToList());
        }
    }
}
