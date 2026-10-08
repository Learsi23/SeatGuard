using static SeatGuard.Api.Features.Events.HolderLabels;

namespace SeatGuard.Api.Features.Events;

public enum SeatStatus
{
    Available,
    Taken,
    /// <summary>Taken by the visitor who asks.</summary>
    Mine
}

public sealed record EventSummaryResponse(Guid Id, 
    string Name,
    string Venue,
    DateTimeOffset StartsAt,
    int SeatCount,
    int AvailableCount);

public sealed record SeatMapResponse(
    Guid Id,
    string Name,
    string Venue,
    DateTimeOffset StartsAt,
    bool IsSandbox,
    IReadOnlyList<SeatResponse> Seats);

public static class HolderLabels
{
    public const string BotPrefix = "bot-";
    public sealed record SeatResponse(Guid Id, string Row, int Number, string Label, SeatStatus Status,
    string? TakenBy);
    /// <summary>Bots get a visible name; real visitors stay anonymous.</summary>
    public static string? ToPublicLabel(string holderId) =>
    holderId.StartsWith(BotPrefix, StringComparison.Ordinal) ? $"Bot #{holderId[BotPrefix.Length..]}"
    : null;
}
