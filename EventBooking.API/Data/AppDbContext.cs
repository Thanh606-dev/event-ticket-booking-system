
using Microsoft.EntityFrameworkCore;
using EventBooking.API.Models;

namespace EventBooking.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(
            DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Event> Events { get; set; } = null!;

        public DbSet<TicketType> TicketTypes { get; set; } = null!;

        public DbSet<Booking> Bookings { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.TicketType)
                .WithMany()
                .HasForeignKey(b => b.TicketTypeId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Booking>()
                .Property(b => b.IdempotencyKey)
                .HasMaxLength(100);

            modelBuilder.Entity<Booking>()
                .HasIndex(b => b.IdempotencyKey)
                .IsUnique()
                .HasFilter("[IdempotencyKey] IS NOT NULL");

        }
    }
}
