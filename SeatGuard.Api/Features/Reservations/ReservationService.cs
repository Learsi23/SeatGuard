using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SeatGuard.Api.Data;
using SeatGuard.Api.Infrastructure;
using SeatGuard.Api.RealTime;

namespace SeatGuard.Api.Features.Reservations;

public enum ReservationOutcome
{
    Created,
    /// <summary>The same holder asked again (for example a retry after a network error). Not an error.</summary>
    AlreadyYours,
    Taken,
    SeatNotFound,
    /// <summary>The holder already has the maximum number of seats.</summary>
    LimitReached
}
/// <summary>The rules for one kind of holder.</summary>
/// <param name="Duration">How long the hold lasts.</param>
/// <param name="MaxHolds">Max seats per holder at the same time; null = no limit (stress-test bots).</param>
/// <param name="AllowSandbox">Visitors may never touch a stress test's private seats; bots must.</param>
public sealed record HoldRules(TimeSpan Duration, int? MaxHolds, bool AllowSandbox);

public partial class ReservationService(AppDbContext db,ISeatNotifier notifier,TimeProvider clock,IOptions<SeatGuardOptions> options,ILogger<ReservationService> logger)
{
    /// <summary>The rules for a real visitor on a shared map.</summary>
    public HoldRules VisitorRules =>
        new(options.Value.HoldDuration, options.Value.MaxHoldsPerVisitor, AllowSandbox: false);
    public Task<ReservationOutcome> ReserveAsync(Guid eventId, Guid seatId, string holderId,
CancellationToken ct = default) =>
        ReserveAsync(eventId, seatId, holderId, VisitorRules, ct);
    public async Task<ReservationOutcome> ReserveAsync(
        Guid eventId, Guid seatId, string holderId, HoldRules rules, CancellationToken ct = default)
    {
        var now = clock.GetUtcNow();
        int rows;
        if (rules.MaxHolds is { } maxHolds)
        {
            // The hold limit is "count my seats, then insert": a check-then-act. Two requests from
            // the SAME visitor at the same moment could both see 3 and both insert. An advisory lock
            // per holder makes one of them wait for the other. Different visitors never wait for
            // each other, and the seat itself is still decided by the unique index.
            await using var transaction = await db.Database.BeginTransactionAsync(ct);
            await db.Database.ExecuteSqlAsync($"SELECT pg_advisory_xact_lock(hashtextextended({holderId}, 0))", ct);
            rows = await TryInsertAsync(eventId, seatId, holderId, rules, maxHolds, now, ct);
            await transaction.CommitAsync(ct);
        }
        else
        {
            rows = await TryInsertAsync(eventId, seatId, holderId, rules, int.MaxValue, now, ct);
        }
        if (rows == 1)
        {
            LogReserved(seatId, holderId);
            await notifier.SeatReservedAsync(eventId, seatId, holderId, ct);
            return ReservationOutcome.Created;
        }
        // Nothing was written. Find out why; these are only reads, the decision is already made.
        var currentHolder = await db.Reservations
            .AsNoTracking()
            .Where(r => r.SeatId == seatId && r.EventId == eventId && r.ExpiresAt > now)
            .Select(r => r.HolderId)
            .FirstOrDefaultAsync(ct);
        if (currentHolder == holderId)
            return ReservationOutcome.AlreadyYours;
        if (currentHolder is not null)
            return ReservationOutcome.Taken;
        var seatIsBookable = await db.Events
            .Where(e => e.Id == eventId && (rules.AllowSandbox || !e.IsSandbox))
            .SelectMany(e => e.Seats)
            .AnyAsync(s => s.Id == seatId, ct);
        if (!seatIsBookable)
            return ReservationOutcome.SeatNotFound;
        if (rules.MaxHolds is { } max && await CountActiveHoldsAsync(holderId, now, ct) >= max)
            return ReservationOutcome.LimitReached;
        // The seat was released a microsecond ago: "taken" is the safe answer, and the client may retry.
        return ReservationOutcome.Taken;
    }
    /// <summary>
    /// ONE atomic SQL statement decides the seat. The unique index on reservations.seat_id is the referee:
    ///  * the first INSERT for a seat wins;
    ///  * every other INSERT for the same seat hits ON CONFLICT and changes nothing (0 rows);
    ///  * exception: if the existing hold has expired, DO UPDATE ... WHERE takes it over.
    /// The values in { } become SQL parameters, never pasted text, so SQL injection is impossible.
    /// </summary>
    private Task<int> TryInsertAsync(
        Guid eventId, Guid seatId, string holderId, HoldRules rules, int maxHolds, DateTimeOffset now,
CancellationToken ct)
    {
        var expiresAt = now + rules.Duration;
        var reservationId = Guid.CreateVersion7(now);
        return db.Database.ExecuteSqlAsync($"""
            INSERT INTO reservations (id, seat_id, event_id, holder_id, created_at, expires_at)
            SELECT {reservationId}, s.id, s.event_id, {holderId}, {now}, {expiresAt}
            FROM seats AS s
            JOIN events AS e ON e.id = s.event_id
            WHERE s.id = {seatId}
              AND s.event_id = {eventId}
              AND (e.is_sandbox = FALSE OR {rules.AllowSandbox})
              AND (SELECT COUNT(*) FROM reservations AS mine
                   WHERE mine.holder_id = {holderId} AND mine.expires_at > {now}) < {maxHolds}
            ON CONFLICT (seat_id) DO UPDATE
                SET id = EXCLUDED.id,
                    holder_id = EXCLUDED.holder_id,
                    created_at = EXCLUDED.created_at,
                    expires_at = EXCLUDED.expires_at
                WHERE reservations.expires_at <= EXCLUDED.created_at
            """, ct);
    }
    private Task<int> CountActiveHoldsAsync(string holderId, DateTimeOffset now, CancellationToken ct) =>
        db.Reservations.CountAsync(r => r.HolderId == holderId && r.ExpiresAt > now, ct);
    /// <returns>False when the holder has no reservation on that seat.</returns>
    public async Task<bool> ReleaseAsync(Guid eventId, Guid seatId, string holderId, CancellationToken ct
= default)
    {
        // The holder condition is part of the DELETE: you can only release your own seat.
        var deleted = await db.Reservations
            .Where(r => r.SeatId == seatId && r.EventId == eventId && r.HolderId == holderId)
            .ExecuteDeleteAsync(ct);
        if (deleted == 0)
            return false;
        LogReleased(seatId, holderId);
        await notifier.SeatReleasedAsync(eventId, seatId, ct);
        return true;
    }
    // Source-generated logging: no string is built when Debug is switched off.
    [LoggerMessage(Level = LogLevel.Debug, Message = "Seat {SeatId} reserved by {HolderId}")]
    private partial void LogReserved(Guid seatId, string holderId);

    [LoggerMessage(Level = LogLevel.Information, Message = "Seat {SeatId} released by {HolderId}")]

    private partial void LogReleased(Guid seatId, string holderId);
}
