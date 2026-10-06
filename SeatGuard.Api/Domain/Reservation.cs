namespace SeatGuard.Api.Domain;

/// <summary>
/// One row = one seat is taken. The unique index on SeatId makes a second row for the
/// same seat impossible: that is the whole "never sold twice" guarantee.
/// </summary>
public sealed class Reservation
{
    public Guid Id { get; init; }
    public Guid SeatId { get; init; }

    /// <summary>Copied from the seat so the seat map can be read with one simple query.</summary>
    public Guid EventId { get; init; }

    /// <summary>The visitor id, or "bot-17" for a stress-test bot.</summary>
    public required string HolderId { get; init; }
    public DateTimeOffset CreatedAt { get; init; }

    /// <summary>After this moment the seat counts as free again.</summary>
    public DateTimeOffset ExpiresAt { get; init; }

}
