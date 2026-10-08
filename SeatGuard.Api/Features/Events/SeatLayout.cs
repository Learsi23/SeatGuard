// Import the domain models namespace where the Seat class is defined
using SeatGuard.Api.Domain;

// Define the namespace for event-related API features using C# 10 file-scoped namespace syntax
namespace SeatGuard.Api.Features.Events;

/// <summary>
/// Provides utility methods to generate seat layouts for events.
/// </summary>
public sealed class SeatLayout
{
    /// <summary>
    /// Generates a list of seats in grid layout: Rows named A, B, C... and seats numbered 1..seatsPerRow.
    /// </summary>
    /// <param name="eventId">The unique identifier of the target event.</param>
    /// <param name="rows">Total number of rows to create (from 1 to 26).</param>
    /// <param name="seatsPerRow">Total number of seats per row (must be at least 1).</param>
    /// <param name="idFactory">Optional custom factory function to generate specific Guids per row and seat index.</param>
    /// <returns>A populated list of Seat objects.</returns>
    public static List<Seat> Create(Guid eventId, int rows, int seatsPerRow, Func<int, int, Guid>? idFactory = null)
    {
        // Throw an ArgumentOutOfRangeException if rows is 0 or negative
        ArgumentOutOfRangeException.ThrowIfLessThan(rows, 1);

        // Throw an ArgumentOutOfRangeException if rows exceeds 26 (restricting rows to single letters A through Z)
        ArgumentOutOfRangeException.ThrowIfGreaterThan(rows, 26);   // A..Z

        // Throw an ArgumentOutOfRangeException if seatsPerRow is 0 or negative
        ArgumentOutOfRangeException.ThrowIfLessThan(seatsPerRow, 1);

        // Pre-allocate the list capacity (rows * seatsPerRow) to optimize memory allocation
        var seats = new List<Seat>(rows * seatsPerRow);

        // Outer loop: Iterate through row indices starting from 0 up to (rows - 1)
        for (var row = 0; row < rows; row++)
        {
            // Inner loop: Iterate through seat numbers in the current row starting from 1 up to seatsPerRow
            for (var number = 1; number <= seatsPerRow; number++)
            {
                // Instantiate a new Seat object and append it to the seats collection
                seats.Add(new Seat
                {
                    // Assign seat ID using the custom factory if supplied; otherwise, fall back to a random Guid
                    Id = idFactory?.Invoke(row, number) ?? Guid.NewGuid(),

                    // Assign the parent Event ID
                    EventId = eventId,

                    // Map the 0-based row integer offset to an uppercase ASCII letter ('A' + 0 = 'A', 'A' + 1 = 'B', etc.)
                    Row = ((char)('A' + row)).ToString(),

                    // Assign the 1-based seat number
                    Number = number
                });
            }
        }

        // Return the complete list of generated seats
        return seats;
    }
}