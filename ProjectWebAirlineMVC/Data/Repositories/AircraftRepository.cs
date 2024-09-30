using Microsoft.EntityFrameworkCore;
using ProjectWebAirlineMVC.Data.Entities;
using ProjectWebAirlineMVC.Data.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWebAirlineMVC.Data.Repositories
{
    public class AircraftRepository : GenericRepository<Aircraft>, IAircraftRepository
    {
        private readonly DataContext _context;


        public AircraftRepository(DataContext context)
            : base(context)
        {
            _context = context;
        }



        public IQueryable GetAllWithUsers()
        {
            return _context.Aircrafts.Include(p => p.User);
        }


        public Task<List<Aircraft>> GetAircraftListAsync()
        {
            return _context.Aircrafts.ToListAsync();
        }

    }
}
