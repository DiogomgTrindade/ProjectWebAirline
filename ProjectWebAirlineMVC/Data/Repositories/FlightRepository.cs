using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectWebAirlineMVC.Data.Entities;
using ProjectWebAirlineMVC.Data.Interfaces;
using ProjectWebAirlineMVC.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWebAirlineMVC.Data.Repositories
{
    public class FlightRepository : GenericRepository<Flight>, IFlightRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IAircraftRepository _aircraftRepository;

        public FlightRepository(DataContext context, IUserHelper userHelper, IAircraftRepository aircraftRepository) : base(context)
        {
            _context = context;
            _userHelper = userHelper;
            _aircraftRepository = aircraftRepository;
        }



        public async Task<IEnumerable<Flight>> GetAllFlightsWithCountriesAsync()
        {
            return await _context.Flights
                .Include(f => f.OriginCountry)
                .Include(f => f.DestinationCountry)
                .ToListAsync();
        }



        public IQueryable GetAllWithUsersAndAircrafts()
        {
            return _context.Flights
                .Include(f => f.User)
                .OrderBy(f => f.Date);
        }

        public async Task<Flight> GetByIdWithCountries(int id)
        {
            return await _context.Flights
               .Include(f => f.User)
               .Include(f => f.OriginCountry)
               .Include(f => f.DestinationCountry)
               .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<Flight> GetByIdWithUsersAndAircraftsAsync(int id)
        {
            return await _context.Flights
                .Include(f => f.User)
                .FirstOrDefaultAsync(f => f.Id == id);
        }


        public async Task<IEnumerable<Flight>> GetAllFlightsWithCountriesAndAircraftAsync()
        {
            return await _context.Flights
                .Include(f => f.OriginCountry)
                .Include(f => f.DestinationCountry)
                .Include(f => f.Aircraft)
                .ToListAsync();
        }


        public IQueryable GetAllWithCountriesAndAircrafts()
        {
            return _context.Flights
                .Include(f => f.User)
                .Include(f => f.Aircraft)
                .Include(f => f.OriginCountry)
                .Include(f => f.DestinationCountry)
                .OrderBy(f => f.Date);
        }



    }
}
