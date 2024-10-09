using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectWebAirlineMVC.Data;
using ProjectWebAirlineMVC.Data.Entities;
using ProjectWebAirlineMVC.Data.Interfaces;
using ProjectWebAirlineMVC.Helpers;
using ProjectWebAirlineMVC.Models;
using NotFoundViewResult = ProjectWebAirlineMVC.Helpers.NotFoundViewResult;

namespace ProjectWebAirlineMVC.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private readonly ITicketRepository _ticketsRepository;
        private readonly IFlightRepository _flightRepository;
        private readonly UserManager<User> _userManager;
        private readonly IMailHelper _mailHelper;

        public TicketsController(ITicketRepository ticketsRepository, IFlightRepository flightRepository, UserManager<User> userManager, IMailHelper mailHelper)
        {
            _ticketsRepository = ticketsRepository;
            _flightRepository = flightRepository;
            _userManager = userManager;
            _mailHelper = mailHelper;
        }

        // GET: Tickets for the current logged-in user
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return new NotFoundViewResult("TicketListNotFound");
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
            var flights = await _flightRepository.GetAllFlightsWithCountriesAndAircraftAsync();
            var flight = flights.FirstOrDefault(f => f.Id == flightId);
            if (flight == null)
            {
                return NotFound();
            }

            var availableTickets = await _ticketsRepository.GetAll()
                                                           .Where(t => t.FlightId == flight.Id && t.IsAvailable)
                                                           .ToListAsync();

            if (!availableTickets.Any())
            {
                return View("NoTicketsAvailable");
            }

            var seatOptions = availableTickets.OrderBy(t => int.Parse(new string(t.Seat.Where(char.IsDigit).ToArray())))
                .Select(t => new SelectListItem
            {
                Value = t.Seat,
                Text = t.Seat
            }).ToList();

            var model = new TicketPurchaseViewModel
            {
                Flight = flight,
                AvailableSeats = seatOptions
            };

            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> BuyTickets(TicketPurchaseViewModel model)
        {

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var availableTickets = await _ticketsRepository.GetTicketsByFlightIdAsync(model.FlightId);
            var ticket = availableTickets.FirstOrDefault(t => t.Seat == model.SelectedSeat);

            if (ticket == null || !ticket.IsAvailable)
            {
                return NotFound();
            }

            
            ticket.PassengerId = user.Id;
            ticket.IsAvailable = false;
            ticket.PassengerFirstName = model.PassengerFirstName;
            ticket.PassengerLastName = model.PassengerLastName;
            ticket.PassengerPhoneNumber = model.PassengerPhoneNumber;
            ticket.PassengerEmail = model.PassengerEmail;
            ticket.HasLuggage = model.HasLuggage;
            ticket.ExtraLuggage = model.ExtraLuggage;

            await _ticketsRepository.UpdateAsync(ticket);

            return RedirectToAction(nameof(PurchaseConfirmed));
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


        public IActionResult PurchaseConfirmed()
        {
            return View();
        }

        public IActionResult NoTicketsAvailable()
        {
            return View();
        }

    }
}
