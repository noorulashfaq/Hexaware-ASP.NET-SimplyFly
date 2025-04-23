using Microsoft.EntityFrameworkCore;
using SimplyFlyServer.Models;

namespace SimplyFlyServer.Context
{
    public class SimplyFlyContext : DbContext
    {
        public SimplyFlyContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Aircraft> Aircrafts { get; set; }
        public DbSet<FlightRoute> Routes { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Airline> Airlines { get; set; }
        public DbSet<FlightRoute> FlightRoutes { get; set; }

        public DbSet<Price> Prices { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Cancellation> Cancellations { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Notification> Notifications { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>();

            modelBuilder.Entity<Airline>()
                .HasOne(a => a.Owner)
                .WithOne(u => u.AirlineAssociated)
                .HasForeignKey<Airline>(a => a.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Aircraft>()
                .HasOne(a => a.Airline)
                .WithMany(al => al.Aircrafts)
                .HasForeignKey(a => a.AirlineId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Flight>()
                .HasOne(f => f.Aircraft)
                .WithMany(a => a.Flights)
                .HasForeignKey(f => f.AircraftId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Flight>()
                .HasOne(f => f.FlightRoute)
                .WithMany(r => r.Flyings)
                .HasForeignKey(f => f.RouteId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Flight>()
                .HasOne(f => f.Airline)
                .WithMany()
                .HasForeignKey(f => f.AirlineId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Payment)
                .WithMany()
                .HasForeignKey(b => b.PaymentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Flight)
                .WithMany()
                .HasForeignKey(b => b.FlightId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cancellation>()
                .HasOne(c => c.Booking)
                .WithMany()
                .HasForeignKey(c => c.BookingId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Seat>()
                .HasOne(s => s.Booking)
                .WithMany()
                .HasForeignKey(s => s.BookingId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Seat>()
                .HasOne(s => s.Flight)
                .WithMany()
                .HasForeignKey(s => s.FlightId)
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
        }


    }
}
