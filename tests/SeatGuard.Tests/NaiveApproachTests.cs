

using Npgsql;
using Org.BouncyCastle.Asn1.X509;
using SeatGuard.Tests.Infrastructure;
using static System.Net.Mime.MediaTypeNames;

namespace SeatGuard.Tests;
/// <summary>
/// Proves the PROBLEM, not our solution. A "check, then insert" reservation without a unique index
/// sells the same seat several times as soon as requests arrive at the same moment.
/// This test is the reason the real code looks the way it does.
/// </summary>

[Collection(ApiCollection.Name)]
public sealed class NaiveApproachTests(SeatGuardFactory factory)
{

    private readonly CancellationToken _ct = TestContext.Current.CancellationToken;


    [Fact]
    public async Task CheckThenInsert_WithoutUniqueIndex_SellsSeatsTwice()
    {

        await using var dataSource = NpgsqlDataSource.Create(factory.ConnectionString);
        await using (var create = dataSource.CreateCommand(
        "DROP TABLE IF EXISTS naive_reservations; CREATE TABLE naive_reservations (seat_id int NOT NULL, holder text NOT NULL); "))
        {
            await create.ExecuteNonQueryAsync(_ct);
        }

        const int seats = 20;
        const int attempts = 200;
        var confirmations = 0;
        var startGun = new TaskCompletionSource();

        var bots = Enumerable.Range(0, attempts).Select(bot => Task.Run(async () =>
        {
            await startGun.Task;
            var seat = bot % seats;
            // Step 1: "is the seat free?"
            await using var check = dataSource.CreateCommand("SELECT COUNT(*) FROM naive_reservations WHERE seat_id = $1");
            check.Parameters.AddWithValue(seat);
            var taken = (long)(await check.ExecuteScalarAsync(_ct))!;
            // ... another request can run its own step 1 right here, and also see "free" ...
            // Step 2: "then it is mine"
            if (taken == 0)
            {
                await using var insert = dataSource.CreateCommand("INSERT INTO naive_reservations (seat_id, holder) VALUES($1, $2)");
                insert.Parameters.AddWithValue(seat);
                insert.Parameters.AddWithValue($"bot-{bot}");
                await insert.ExecuteNonQueryAsync(_ct);
                Interlocked.Increment(ref confirmations);
            }
        }, _ct)).ToList();
        startGun.SetResult();
        await Task.WhenAll(bots);

        // More "you got it!" answers than there are seats: some seats were sold twice.
        TestContext.Current.SendDiagnosticMessage($"Naive approach: {confirmations} confirmations for { seats} seats");
        Assert.True(confirmations > seats, $"Expected double bookings, got {confirmations} confirmations for { seats} seats");
    }



}
