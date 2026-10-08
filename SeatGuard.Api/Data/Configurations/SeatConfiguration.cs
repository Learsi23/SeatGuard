using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeatGuard.Api.Domain;

namespace SeatGuard.Api.Data.Configurations;

public sealed class SeatConfiguration : IEntityTypeConfiguration<Seat>
{
    public void Configure(EntityTypeBuilder<Seat> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Row).HasMaxLength(2);

        // Two "A12" in the same event would be a data error.

        builder.HasIndex(s => new { s.EventId, s.Row, s.Number }).IsUnique();

        builder.HasData(DemoEvent.CreateSeats());
    }
}
