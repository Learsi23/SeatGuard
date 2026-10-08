using SeatGuard.Api.Features.Events;

namespace SeatGuard.Api.RealTime;

/// <summary>A seat became taken or free. TakenBy is a public label ("Bot #17") or null.</summary>
public sealed record SeatChangedMessage(Guid EventId, Guid SeatId, SeatStatus Status, string? TakenBy);
public sealed record StressTestProgressMessage(
Guid RunId,
int Attempts,
int Completed,
int Succeeded,
int Conflicts,
int Failed);
public sealed record StressTestCompletedMessage(
Guid RunId,
int Attempts,
int SeatCount,
int Succeeded,
int Conflicts,
int Failed,
int DoubleBookings,
int DurationMs);
