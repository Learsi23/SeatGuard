namespace SeatGuard.Api.Domain;

public sealed class StressTestRun
{

    public Guid Id { get; init; }
    /// <summary>The sandbox event the bots compete for.</summary>
    public Guid EventId { get; init; }
    public required string StartedBy { get; init; }
    public int Attempts { get; init; }
    public int SeatCount { get; init; }
    public StressTestStatus Status { get; set; }
    public int Succeeded { get; set; }
    public int Conflicts { get; set; }
    public int Failed { get; set; }
    /// <summary>Measured from the database after the run. Must always be 0.</summary>
    public int DoubleBookings { get; set; }
    public int DurationMs { get; set; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? CompletedAt { get; set; }

}
public enum StressTestStatus
{
    /// <summary>The sandbox exists; waiting for its owner to press Start.</summary>
    Ready = 1,
    Queued = 2,
    Running = 3,
    Completed = 4,
    Failed = 5
}