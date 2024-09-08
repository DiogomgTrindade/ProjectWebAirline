using ProjectWebAirlineMVC.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWebAirlineMVC.Data
{
    public class Repository : IRepository
    {
        private readonly DataContext _context;

        public Repository(DataContext context)
        {
            _context = context;
        }


        public void AddAircraft(Aircraft aircraft)
        {
            _context.Aircrafts.Add(aircraft);
        }

        public bool AircraftExists(int id)
        {
            return _context.Aircrafts.Any(a => a.Id == id);
        }

        public Aircraft GetAircraft(int id)
        {
            return _context.Aircrafts.Find(id);
        }

        public IEnumerable<Aircraft> GetAircrafts()
        {
            return _context.Aircrafts.ToList();
        }

        public void RemoveAircraft(Aircraft aircraft)
        {
            _context.Aircrafts.Remove(aircraft);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void UpdateAircraft(Aircraft aircraft)
        {
            _context.Aircrafts.Update(aircraft);
        }
    }
}
