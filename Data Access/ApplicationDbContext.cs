using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace SkyLine.Data_Access
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<UserOTP> UserOTPs { get; set; }
        public DbSet<LoyalitySystem> LoyalitySystems { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<AirPort> AirPorts { get; set; }
        public DbSet<Airline> AirLines { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<FlightSegment> FlightSegments { get; set; }
        public DbSet<Fare> Fares { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingPassenger> BookingPassengers { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Seat_Reservation> Seat_Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Flight>()
                .HasOne(f => f.LeavingAirport)
                .WithMany(a => a.DepartingFlights)
                .HasForeignKey(f => f.Leaving_Airport_ID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Flight>()
                .HasOne(f => f.ArriveAirport)
                .WithMany(a => a.ArrivingFlights)
                .HasForeignKey(f => f.Arrive_Airport_ID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FlightSegment>()
                .HasOne(fs => fs.DepartureAirport)
                .WithMany(a => a.DepartingSegments)
                .HasForeignKey(fs => fs.Departure_Airport_ID_Fk)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FlightSegment>()
                .HasOne(fs => fs.ArrivalAirport)
                .WithMany(a => a.ArrivingSegments)
                .HasForeignKey(fs => fs.Arrival_Airport_ID_Fk)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Flights)
                .WithMany()
                .HasForeignKey(b => b.Flight_Id)
                .OnDelete(DeleteBehavior.NoAction);


        }




    }
}
