using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeatGuard.Api.Domain;

namespace SeatGuard.Api.Data.Configurations
{
    public sealed class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Name).HasMaxLength(200);
            builder.Property(e => e.Venue).HasMaxLength(200);

            // An event owns its seats: deleting a sandbox event deletes its seats (and, through them, the reservations).

            builder.HasMany(e => e.Seats)
                .WithOne()
                .HasForeignKey(s => s.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            // The cleanup job looks for old sandbox events

            builder.HasIndex(e => new { e.IsSandbox, e.StartsAt });

            builder.HasData(DemoEvent.Create());

        }
    }
}
