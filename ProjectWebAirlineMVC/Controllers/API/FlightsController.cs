using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectWebAirlineMVC.Data.Interfaces;
using System.Threading.Tasks;

namespace ProjectWebAirlineMVC.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly IFlightRepository _flightRepository;

        public FlightsController(IFlightRepository flightRepository)
        {
            _flightRepository = flightRepository;
        }


        [HttpGet]
        public async Task<IActionResult> GetFlights()
        {
            return Ok(await _flightRepository.GetAllFlightsWithCountriesAsync());
        }


    }
}
