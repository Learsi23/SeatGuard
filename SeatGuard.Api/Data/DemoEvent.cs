using SeatGuard.Api.Domain;
using SeatGuard.Api.Features.Events;

namespace SeatGuard.Api.Data
{
    /// <summary>
    /// The shared concert that every visitor sees on the start page. Inserted by a migration (HasData),
    /// so every value must be fixed: no Guid.NewGuid(), no DateTime.UtcNow.
    /// </summary>
    public static class DemoEvent
    {


        public static readonly Guid Id = new("5ea7c0de-0000-4000-8000-000000000001");
        public const int Rows = 10;
        public const int SeatsPerRow = 20;
        public static Event Create() => new()
        {
            Id = Id,
            Name = "SeatGuard Live: Demo Night",
            Venue = "Avicii Arena, Stockholm",
            StartsAt = new DateTimeOffset(2026, 12, 12, 19, 0, 0, TimeSpan.Zero),
            IsSandbox = false,
            CreatedAt = new DateTimeOffset(2026, 10, 1, 0, 0, 0, TimeSpan.Zero),
        };
        // Deterministic ids: the same seat always gets the same Guid, so the migration never changes.
        public static List<Seat> CreateSeats() =>
        SeatLayout.Create(Id, Rows, SeatsPerRow, (row, number) => new Guid($"5ea7c0de-0001-4000-8000-{row:x4}{number:x8}"));
    }
}
