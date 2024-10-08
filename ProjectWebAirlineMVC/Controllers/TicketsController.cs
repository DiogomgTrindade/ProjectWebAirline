using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectWebAirlineMVC.Data;
using ProjectWebAirlineMVC.Data.Entities;
using ProjectWebAirlineMVC.Data.Interfaces;
using ProjectWebAirlineMVC.Helpers;
using ProjectWebAirlineMVC.Models;

namespace ProjectWebAirlineMVC.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ITicketRepository _ticketsRepository;
        private readonly IFlightRepository _flightRepository;
        private readonly UserManager<User> _userManager;

        public TicketsController(ITicketRepository ticketsRepository, IFlightRepository flightRepository, UserManager<User> userManager)
        {
            _ticketsRepository = ticketsRepository;
            _flightRepository = flightRepository;
            _userManager = userManager;
        }

        // GET: Tickets for the current logged-in user
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            var tickets = await _ticketsRepository.GetAll()
                                                  .Where(t => t.PassengerId == user.Id)
                                                  .Include(t => t.Flight) 
                                                    .ThenInclude(f => f.OriginCountry)
                                                  .Include(t => t.Flight)
                                                    .ThenInclude(f => f.DestinationCountry)
                                                  .Include(t => t.Flight)
                                                    .ThenInclude(f => f.Aircraft)
                                                  .ToListAsync();

            return View(tickets);
        }

        public async Task<IActionResult> TicketList(int flightId)
        {
            var flight = await _flightRepository.GetByIdAsync(flightId);
            if (flight == null)
            {
                return NotFound();
            }

            var availableTickets = await _ticketsRepository.GetAll()
                                                           .Where(t => t.FlightId == flight.Id && t.IsAvailable)
                                                           .ToListAsync();

            if (!availableTickets.Any())
            {
                return NotFound();
            }

            var model = new TicketListViewModel
            {
                Flight = flight,
                AvailableTickets = availableTickets
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> BuyTickets(int ticketId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var ticket = await _ticketsRepository.GetByIdAsync(ticketId);
            if (ticket == null || !ticket.IsAvailable)
            {
                return NotFound();
            }

            ticket.PassengerId = user.Id;
            ticket.IsAvailable = false;

            await _ticketsRepository.UpdateAsync(ticket);
            return RedirectToAction(nameof(Index));
        }


        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _ticketsRepository.GetAll()
                                          .Include(t => t.Flight)
                                             .ThenInclude(f => f.OriginCountry)
                                          .Include(t => t.Flight)
                                             .ThenInclude(f => f.DestinationCountry)
                                          .Include(t => t.Flight)
                                             .ThenInclude(f => f.Aircraft)
                                          .FirstOrDefaultAsync(t => t.Id == id.Value);

            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }


    }
}
