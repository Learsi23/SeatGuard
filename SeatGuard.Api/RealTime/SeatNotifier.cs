using Microsoft.AspNetCore.SignalR;
using SeatGuard.Api.Features.Events;

namespace SeatGuard.Api.RealTime;

/// <summary>
/// The rest of the app talks to this interface, not to SignalR directly:
/// the services stay testable and do not know how messages travel.
/// </summary>
public interface ISeatNotifier
{
    Task SeatReservedAsync(Guid eventId, Guid seatId, string holderId, CancellationToken ct);
    Task SeatReleasedAsync(Guid eventId, Guid seatId, CancellationToken ct);
    Task StressTestProgressAsync(Guid eventId, StressTestProgressMessage message, CancellationToken ct);
    Task StressTestCompletedAsync(Guid eventId, StressTestCompletedMessage message, CancellationToken
    ct);
}

public sealed class SignalRSeatNotifier(IHubContext<SeatHub, ISeatClient> hub) : ISeatNotifier
{
    // Everyone in the room sees "taken". Whether it is *yours* is decided by each browser
    // (it knows its own reservations); visitor ids are never broadcast.
    public Task SeatReservedAsync(Guid eventId, Guid seatId, string holderId, CancellationToken ct) =>
    Group(eventId).SeatChanged(new SeatChangedMessage(eventId, seatId, SeatStatus.Taken,
    HolderLabels.ToPublicLabel(holderId)));
    public Task SeatReleasedAsync(Guid eventId, Guid seatId, CancellationToken ct) =>
    Group(eventId).SeatChanged(new SeatChangedMessage(eventId, seatId, SeatStatus.Available, null));
    public Task StressTestProgressAsync(Guid eventId, StressTestProgressMessage message,
    CancellationToken ct) =>
    Group(eventId).StressTestProgress(message);
    public Task StressTestCompletedAsync(Guid eventId, StressTestCompletedMessage message,
    CancellationToken ct) =>
    Group(eventId).StressTestCompleted(message);
    private ISeatClient Group(Guid eventId) => hub.Clients.Group(SeatHub.GroupName(eventId));
}