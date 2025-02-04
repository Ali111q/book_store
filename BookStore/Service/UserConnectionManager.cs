using System.Collections.Concurrent;

namespace BookStore.Services;

#region interface
public interface IUserConnectionManager
{
    void AddUser(Guid userId, string connectionId);
    void RemoveUser(Guid userId);
    List<Guid> GetConnectedUsers();
    string? GetConnectionId(Guid userId);
}
#endregion

public class UserConnectionManager : IUserConnectionManager
{
    #region private
    private readonly ConcurrentDictionary<Guid, string> _connectedUsers = new();
    #endregion

    #region add
    public void AddUser(Guid userId, string connectionId)
    {
        _connectedUsers[userId] = connectionId;
    }
    #endregion

    #region remove
    public void RemoveUser(Guid userId)
    {
        _connectedUsers.TryRemove(userId, out _);
    }
    #endregion

    #region get
    public List<Guid> GetConnectedUsers()
    {
        return _connectedUsers.Keys.ToList();
    }

    public string? GetConnectionId(Guid userId)
    {
        _connectedUsers.TryGetValue(userId, out var connectionId);
        return connectionId;
    }
    #endregion
}