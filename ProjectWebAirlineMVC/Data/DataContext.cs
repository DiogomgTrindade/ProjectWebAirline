﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectWebAirlineMVC.Data.Entities;

namespace ProjectWebAirlineMVC.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DbSet<Aircraft> Aircrafts { get; set; }

        public DbSet<Flight> Flights { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
        }

    }
}
