

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SeatGuard.Api.Data;
using SeatGuard.Api.Domain;
using SeatGuard.Api.Features.Events;
using Testcontainers.PostgreSql;

namespace SeatGuard.Tests.Infrastructure;
/// <summary>
/// Starts a real PostgreSQL in Docker (Testcontainers) and the real API in memory.
/// Concurrency can only be tested against the real database: an in-memory fake has no locks or unique indexes.
/// </summary>

public sealed class SeatGuardFactory : WebApplicationFactory<Program>, IAsyncLifetime
{

    private readonly PostgreSqlContainer _database = new PostgreSqlBuilder("postgres:18-alpine").Build();

    // PostgreSQL accepts 100 connections by default. The API's pool and the tests' own pools
    // must fit inside that together, so each one gets a smaller maximum.

    public string ConnectionString => $"{_database.GetConnectionString()};Maximum Pool Size=40";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.UseSetting("ConnectionStrings:SeatGuard", ConnectionString);
        // The tests send many requests quickly; the real limits are tested separately.
        builder.UseSetting("SeatGuard:RequestsPerMinute", "100000");
        builder.UseSetting("SeatGuard:SessionsPerMinute", "100000");
        builder.UseSetting("SeatGuard:StressTestsPerWindow", "100");
    }

    public async ValueTask InitializeAsync()
    {
        await _database.StartAsync();
        // Migrate BEFORE the API starts, exactly like a real deployment does.
        // (On startup the API reads its Data Protection keys from the database.)
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSeatGuardDatabase(ConnectionString)
            .Options;
        await using var db = new AppDbContext(options);
        await db.Database.MigrateAsync();
    }

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await _database.DisposeAsync();
    }

    /// <summary>Every test gets its own event, so tests never share seats.</summary>
    public async Task<TestEvent> CreateEventAsync(int rows = 2, int seatsPerRow = 5)
    {
        var ev = new Event
        {
            Id = Guid.CreateVersion7(),
            Name = "Test event",
            Venue = "Test venue",
            StartsAt = DateTimeOffset.UtcNow,
            CreatedAt = DateTimeOffset.UtcNow,
        };
        ev.Seats.AddRange(SeatLayout.Create(ev.Id, rows, seatsPerRow));
        await using var scope = Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Events.Add(ev);
        await db.SaveChangesAsync();
        return new TestEvent(ev.Id, ev.Seats.Select(s => s.Id).ToList());
    }

     public async Task<T> WithDbAsync<T>(Func<AppDbContext, Task<T>> action)
    {
        await using var scope = Services.CreateAsyncScope();
        return await action(scope.ServiceProvider.GetRequiredService<AppDbContext>());
    }
}

    public sealed record TestEvent(Guid Id, IReadOnlyList<Guid> SeatIds);
    [CollectionDefinition(Name)]
    public sealed class ApiCollection : ICollectionFixture<SeatGuardFactory>
    {
        public const string Name = "Api";
    }




