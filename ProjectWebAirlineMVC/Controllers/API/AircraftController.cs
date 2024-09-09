using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectWebAirlineMVC.Data;

namespace ProjectWebAirlineMVC.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AircraftController : ControllerBase
    {

        private readonly IAircraftRepository _aircraftRepository;

        public AircraftController(IAircraftRepository aircraftRepository)
        {

            _aircraftRepository = aircraftRepository;
        }


        [HttpGet]
        public IActionResult GetAircrafts()
        {
            return Ok(_aircraftRepository.GetAllWithUsers());
        }

    }
}
