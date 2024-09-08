using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectWebAirlineMVC.Data.Entities;

namespace ProjectWebAirlineMVC.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DbSet<Aircraft> Aircrafts { get; set; }



        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

    }
}
