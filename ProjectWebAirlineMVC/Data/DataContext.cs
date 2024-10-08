using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectWebAirlineMVC.Data.Entities;

namespace ProjectWebAirlineMVC.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DbSet<Aircraft> Aircrafts { get; set; }

        public DbSet<Flight> Flights { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Tickets> Tickets { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>()
                .HasIndex(c => c.Name)
                .IsUnique();


            modelBuilder.Entity<Flight>()
                .HasOne(f => f.Aircraft)
                .WithMany()
                .HasForeignKey(f => f.AircraftId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Flight>()
                .HasOne(f => f.OriginCountry)
                .WithMany() 
                .HasForeignKey(f => f.OriginCountryId)
                .OnDelete(DeleteBehavior.Restrict); 


            modelBuilder.Entity<Flight>()
                .HasOne(f => f.DestinationCountry)
                .WithMany() 
                .HasForeignKey(f => f.DestinationCountryId)
                .OnDelete(DeleteBehavior.Restrict);  


            modelBuilder.Entity<Tickets>()
                .HasOne(t => t.Flight)
                .WithMany(f => f.TicketList)
                .HasForeignKey(t => t.FlightId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Tickets>()
                .HasOne(t => t.Passenger)
                .WithMany(u => u.TicketList)
                .HasForeignKey(t => t.PassengerId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }

    }
}
