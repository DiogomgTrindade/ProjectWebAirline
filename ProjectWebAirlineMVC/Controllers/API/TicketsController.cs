using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Modes;
using ProjectWebAirlineMVC.Data.Entities;
using ProjectWebAirlineMVC.Data.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWebAirlineMVC.Controllers.API
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly UserManager<User> _userManager;


        public TicketsController(ITicketRepository ticketRepository, UserManager<User> userManager)
        {
            _ticketRepository = ticketRepository;
            _userManager = userManager;
        }

        [HttpGet("FutureFlights")]
        public async Task<IActionResult> GetFutureFlights()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized("User not found.");
            }

            var futureFlights = await _ticketRepository.GetAll()
                .Where(t => t.PassengerId == user.Id && t.Flight.Date > DateTime.Now)
                .Include(t => t.Flight)
                    .ThenInclude(f => f.OriginCountry)
                .Include(t => t.Flight)
                    .ThenInclude(f => f.DestinationCountry)
                .Include(t => t.Flight)
                    .ThenInclude(f => f.Aircraft)
                .Select(t => new
                {
                    FlightNumber = t.Flight.FlightNumber,
                    Date = t.Flight.Date,
                    Origin =  t.Flight.OriginCountry.Name,
                    Destination = t.Flight.DestinationCountry.Name,
                    Aircraft = t.Flight.Aircraft.Name,
                    Seat = t.Seat
                }).ToListAsync();

            if (!futureFlights.Any())
            {
                return NotFound("Não foram encontrados voos futuros para este cliente.");
            }


            return Ok(futureFlights);
        }


    }
}
