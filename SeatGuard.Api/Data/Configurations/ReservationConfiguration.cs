using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeatGuard.Api.Domain;

namespace SeatGuard.Api.Data.Configurations
{
    public sealed class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.HolderId).HasMaxLength(64);

            // THE guarantee: at most one reservation row per seat. PostgreSQL enforces it,
            // even when 500 requests arrive in the same millisecond.

            builder.HasIndex(r => r.SeatId).IsUnique();

            builder.HasOne<Seat>()
                .WithMany()
                .HasForeignKey(r => r.SeatId)
                .OnDelete(DeleteBehavior.Cascade);

            // The seat map reads all reservations of one event.
            builder.HasIndex(r => r.EventId);
            // The cleanup job deletes expired holds.
            builder.HasIndex(r => r.ExpiresAt);
            // The hold limit counts one visitor's reservations.
            builder.HasIndex(r => r.HolderId);

        }
    }
}
