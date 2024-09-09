using Microsoft.EntityFrameworkCore;
using ProjectWebAirlineMVC.Data.Entities;
using System.Linq;

namespace ProjectWebAirlineMVC.Data
{
    public class AircraftRepository : GenericRepository<Aircraft>, IAircraftRepository
    {
        private readonly DataContext _context;


        public AircraftRepository(DataContext context)
            :base (context)
        {
            _context = context;
        }

        public IQueryable GetAllWithUsers()
        {
            return _context.Aircrafts.Include(p => p.User);
        }
    }
}
