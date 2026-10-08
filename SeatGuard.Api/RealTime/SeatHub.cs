using Microsoft.AspNetCore.SignalR;

namespace SeatGuard.Api.RealTime;


/// <summary>
/// The messages the server can push to browsers. A typed hub turns a typo in a
/// method name into a compile error instead of a message nobody receives.
/// </summary>
public interface ISeatClient
{
    Task SeatChanged(SeatChangedMessage message);
    Task StressTestProgress(StressTestProgressMessage message);
    Task StressTestCompleted(StressTestCompletedMessage message);
}

/// <summary>
/// Browsers connect here and join the "room" (group) of the event they are looking at.
/// The hub only pushes data out; every change goes through the HTTP API.
/// </summary>
public sealed class SeatHub : Hub<ISeatClient>
{
    public Task JoinEvent(Guid eventId) =>
    Groups.AddToGroupAsync(Context.ConnectionId, GroupName(eventId));
    public Task LeaveEvent(Guid eventId) =>
    Groups.RemoveFromGroupAsync(Context.ConnectionId, GroupName(eventId));
    public static string GroupName(Guid eventId) => $"event:{eventId}";
}