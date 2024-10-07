using ProjectWebAirlineMVC.Data.Entities;
using ProjectWebAirlineMVC.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectWebAirlineMVC.Data.Interfaces
{
    public class TicketRepository : GenericRepository<Tickets>, ITicketRepository
    {
        private readonly DataContext _context;

        public TicketRepository(DataContext context) :base(context) 
        {
            _context = context;
        }

        public async Task GenerateTicketsAsync(Flight flight)
        {
            if(flight.Aircraft == null)
            {
                throw new InvalidOperationException("Aircraft is not assigned to this flight.");
            }

            var tickets = new List<Tickets>();

            for(int i =1; i <= flight.Aircraft.Capacity; i++)
            {
                var ticket = new Tickets
                {
                    FlightId = flight.Id,
                    Flight = flight,
                    Seat = "Seat " + i,
                    IsAvailable = true
                };

                tickets.Add(ticket);
            }

            await _context.Tickets.AddRangeAsync(tickets);
            await _context.SaveChangesAsync();
        }
    }
}
