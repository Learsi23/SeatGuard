using System.ComponentModel.DataAnnotations;

namespace SeatGuard.Api.Infrastructure;
/// <summary>All tunable numbers in one place, read from the "SeatGuard" section of appsettings.json.</summary>
public sealed class SeatGuardOptions
{

    public const string SectionName = "SeatGuard";

    /// <summary>How long a visitor's reservation on the shared map is kept.</summary>
    [Range(typeof(TimeSpan), "00:01:00", "1.00:00:00")]
    public TimeSpan HoldDuration { get; init; } = TimeSpan.FromMinutes(10);

    /// <summary>Fairness: one visitor may hold at most this many seats on a shared map at the same time.</summary>
    [Range(1, 200)]
    public int MaxHoldsPerVisitor { get; init; } = 4;

    /// <summary>Global limit per visitor (or IP address when there is no visitor yet).</summary>
    [Range(10, 100_000)]
    public int RequestsPerMinute { get; init; } = 300;

    /// <summary>New visitor sessions per IP address and minute. Stops one machine from minting thousands  of identities.</summary>
    [Range(1, 100_000)]
    public int SessionsPerMinute { get; init; } = 20;
    [Range(1, 100)]
    public int StressTestsPerWindow { get; init; } = 3;
    public TimeSpan StressTestWindow { get; init; } = TimeSpan.FromMinutes(10);
    [Range(1, 26)]
    public int SandboxRows { get; init; } = 5;
    [Range(1, 50)]
    public int SandboxSeatsPerRow { get; init; } = 20;

    /// <summary>Protects the database: at most this many stress-test copies exist at the same time.</summary>
    [Range(1, 1000)]
    public int MaxActiveSandboxes { get; init; } = 20;

    [Range(typeof(TimeSpan), "00:01:00", "7.00:00:00")]
    public TimeSpan SandboxLifetime { get; init; } = TimeSpan.FromHours(1);
    [Range(typeof(TimeSpan), "00:00:01", "01:00:00")]
    public TimeSpan CleanupInterval { get; init; } = TimeSpan.FromSeconds(30);


}
