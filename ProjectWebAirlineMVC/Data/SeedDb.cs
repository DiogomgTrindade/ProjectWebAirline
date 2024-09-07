using ProjectWebAirlineMVC.Data.Entities;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace ProjectWebAirlineMVC.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private Random _random;

        public SeedDb(DataContext context)
        {
            _context = context;
            _random = new Random();
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            if (!_context.Aircrafts.Any())
            {
                AddAircraft("Boeing 737");
                AddAircraft("Airbus A310");
                AddAircraft("Cessna 120");
                AddAircraft("Saab 2000");
                await _context.SaveChangesAsync();
            }
        }


        /// <summary>
        /// Adds a craft
        /// </summary>
        /// <param name="name">Aircraft name</param>
        private void AddAircraft(string name)
        {
            _context.Aircrafts.Add(new Aircraft
            {
                Name = name,
                Capacity = _random.Next(100, 500)
            });
        }
    }
}

