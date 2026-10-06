namespace SeatGuard.Api.Domain;

public sealed class Seat
{
    public Guid Id { get; init; }
    public Guid EventId { get; init; }

    /// <summary>
    /// Row identifier (e.g., "A", "B").
    /// </summary>
    public required string Row { get; init; }

    /// <summary>
    /// Seat number within the row (e.g., 1, 2, 3).
    /// </summary>
    public int Number { get; init; }

    /// <summary>
    /// Formatted seat label (e.g., "A12").
    /// Calculated from <see cref="Row"/> and <see cref="Number"/>; not stored in the database.
    /// </summary>
    //public string Label
    //{
    //    get
    //    {
    //        return $"{Row}{Number}";
    //    }
    //}
    public string Label => $"{Row}{Number}";
}