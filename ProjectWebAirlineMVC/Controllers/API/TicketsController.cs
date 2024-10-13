using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Modes;
using ProjectWebAirlineMVC.Data.Entities;
using ProjectWebAirlineMVC.Data.Interfaces;
using ProjectWebAirlineMVC.Data.Repositories;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProjectWebAirlineMVC.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IFlightRepository _flightRepository;
        private readonly UserManager<User> _userManager;


        public TicketsController(ITicketRepository ticketRepository, UserManager<User> userManager, IFlightRepository flightRepository)
        {
            _ticketRepository = ticketRepository;
            _userManager = userManager;
            _flightRepository = flightRepository;
        }


        [HttpGet]
        public async Task<IActionResult> GetCustomerFlights()
        {
            try
            {

                var emailClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                var userEmail = emailClaim?.Value;

                if (string.IsNullOrEmpty(userEmail))
                {
                    return NotFound("Usuário não encontrado.");
                }


                var tickets = await _ticketRepository.GetTicketsByUserEmailAsync(userEmail);

                if (tickets == null || !tickets.Any())
                {
                    return NotFound("Nenhum voo futuro encontrado para este usuário.");
                }


                var ticketDetails = tickets.Select(t => new
                {
                    t.Id,
                    PassengerName = t.PassengerFirstName + " " + t.PassengerLastName, 
                    t.PassengerId,
                    t.Seat,
                    Flight = new
                    {
                        t.Flight.Id,
                        t.Flight.FlightNumber,
                        t.Flight.Date,
                        Origin = t.Flight.OriginCountry.Name,
                        Destination = t.Flight.DestinationCountry.Name,
                        Aircraft = t.Flight.Aircraft.Name
                    }
                });

                return Ok(ticketDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno no servidor: {ex.Message}");
            }
        }
    }
}
