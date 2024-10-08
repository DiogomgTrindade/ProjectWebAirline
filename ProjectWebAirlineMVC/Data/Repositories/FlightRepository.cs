using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectWebAirlineMVC.Data.Entities;
using ProjectWebAirlineMVC.Data.Interfaces;
using ProjectWebAirlineMVC.Helpers;
using System;
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
        private readonly ITicketRepository _ticketRepository;

        public FlightRepository(DataContext context, IUserHelper userHelper, IAircraftRepository aircraftRepository, ITicketRepository ticketRepository) : base(context)
        {
            _context = context;
            _userHelper = userHelper;
            _aircraftRepository = aircraftRepository;
            _ticketRepository = ticketRepository;

        }



        public async Task<IEnumerable<Flight>> GetAllFlightsWithCountriesAsync()
        {
            return await _context.Flights
                .Include(f => f.OriginCountry)
                .Include(f => f.DestinationCountry)
                .ToListAsync();
        }

        public async Task<IEnumerable<Flight>> GetAllFlightsWithCountriesAndAircraftAsync()
        {
            return await _context.Flights
                .Include(f => f.OriginCountry)
                .Include(f => f.DestinationCountry)
                .Include(f => f.Aircraft)
                .Include(f => f.TicketList)
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
