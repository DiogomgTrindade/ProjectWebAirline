using System;
using System.Collections.Generic;
using System.IO;
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
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
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

        public async Task<IActionResult> DownloadPDF (int? id)
        {
            var ticket = await _ticketsRepository.GetAll()
                                          .Include(t => t.Flight)
                                             .ThenInclude(f => f.OriginCountry)
                                          .Include(t => t.Flight)
                                             .ThenInclude(f => f.DestinationCountry)
                                          .Include(t => t.Flight)
                                             .ThenInclude(f => f.Aircraft)
                                          .FirstOrDefaultAsync(t => t.Id == id);

            if (ticket == null)
            {
                return NotFound();
            }

            using (PdfDocument document = new PdfDocument())
            {
                PdfPage page = document.Pages.Add();
                PdfGraphics graphics = page.Graphics;
                PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 12);

                graphics.DrawString("Ticket Details", font, PdfBrushes.Black, new PointF(0, 0));

                graphics.DrawString("Flight Information", font, PdfBrushes.Black, new PointF(0, 40));
                graphics.DrawString($"Flight Number: {ticket.Flight.FlightNumber}", font, PdfBrushes.Black, new PointF(0, 60));
                graphics.DrawString($"Aircraft: {ticket.Flight.Aircraft.Name}", font, PdfBrushes.Black, new PointF(0, 80));
                graphics.DrawString($"Origin: {ticket.Flight.OriginCountry.Name}", font, PdfBrushes.Black, new PointF(0, 100));
                graphics.DrawString($"Destination: {ticket.Flight.DestinationCountry.Name}", font, PdfBrushes.Black, new PointF(0, 120));
                graphics.DrawString($"Date: {ticket.Flight.Date.ToString("dd/MM/yyyy")}", font, PdfBrushes.Black, new PointF(0, 140));

                graphics.DrawString("Ticket Information", font, PdfBrushes.Black, new PointF(0, 180));
                graphics.DrawString($"Seat: {ticket.Seat}", font, PdfBrushes.Black, new PointF(0, 200));
                graphics.DrawString($"Passenger: {ticket.PassengerFirstName} {ticket.PassengerLastName}", font, PdfBrushes.Black, new PointF(0, 220));
                graphics.DrawString($"Phone: {ticket.PassengerPhoneNumber}", font, PdfBrushes.Black, new PointF(0, 240));
                graphics.DrawString($"Email: {ticket.PassengerEmail}", font, PdfBrushes.Black, new PointF(0, 260));
                graphics.DrawString($"Luggage: {(ticket.HasLuggage ? "Yes" : "No")}", font, PdfBrushes.Black, new PointF(0, 280));
                graphics.DrawString($"Extra Luggage: {(ticket.ExtraLuggage ? "Yes" : "No")}", font, PdfBrushes.Black, new PointF(0, 300));

                MemoryStream stream = new MemoryStream();
                document.Save(stream);
                stream.Position = 0;

                return File(stream, "application/pdf", $"{ticket.PassengerFirstName}_{ticket.PassengerLastName}_Details.pdf");
            }

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
