using Microsoft.EntityFrameworkCore;
using ProjectWebAirlineMVC.Data.Entities;

namespace ProjectWebAirlineMVC.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Aircraft> Aircrafts { get; set; }



        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

    }
}
