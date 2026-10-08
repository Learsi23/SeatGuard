namespace SeatGuard.Api.Domain;

public sealed class Event
{
    public Guid Id { get; init; }
    public required string Name { get; set; }
    public required string Venue { get; set; }
    public DateTimeOffset StartsAt { get; set; }
    /// <summary>True for the private copy of the seat map that a stress test creates. Deleted after a while.</summary>
    public bool IsSandbox { get; init; }

    public DateTimeOffset CreatedAt { get; init; }
    public List<Seat> Seats { get; init; } = [];
}
