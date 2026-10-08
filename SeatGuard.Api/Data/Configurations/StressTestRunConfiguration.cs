using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeatGuard.Api.Domain;
namespace SeatGuard.Api.Data.Configurations;

public sealed class StressTestRunConfiguration : IEntityTypeConfiguration<StressTestRun>
{
    public void Configure(EntityTypeBuilder<StressTestRun> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.StartedBy).HasMaxLength(64);

        // "Completed" instead of 3 in the table: readable in SQL, and safe if the enum is reordered.
        builder.Property(r => r.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.HasOne<Event>()
            .WithMany()
            .HasForeignKey(r => r.EventId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}