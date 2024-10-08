using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectWebAirlineMVC.Data.Entities;
using ProjectWebAirlineMVC.Data.Interfaces;
using ProjectWebAirlineMVC.Helpers;
using ProjectWebAirlineMVC.Models;
using NotFoundViewResult = ProjectWebAirlineMVC.Helpers.NotFoundViewResult;

namespace ProjectWebAirlineMVC.Controllers
{
    public class FlightsController : Controller
    {
        private readonly IFlightRepository _flightRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IAircraftRepository _aircraftRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IUserHelper _userHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;

        public FlightsController(IFlightRepository flightRepository, ICountryRepository countryRepository, IUserHelper userHelper, IBlobHelper blobHelper, IConverterHelper converterHelper, IAircraftRepository aircraftRepository, ITicketRepository ticketRepository)
        {
            _flightRepository = flightRepository;
            _countryRepository = countryRepository;
            _aircraftRepository = aircraftRepository;
            _ticketRepository = ticketRepository;
            _userHelper = userHelper;
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
        }

        // GET: Flights
        public async Task<IActionResult> Index()
        {
            var flights = await _flightRepository.GetAllFlightsWithCountriesAndAircraftAsync();
            return View(flights);
        }


        // GET: Flights/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("FlightNotFound");
            }

            var flights = await _flightRepository.GetAllFlightsWithCountriesAndAircraftAsync();
            var flight = flights.FirstOrDefault(f => f.Id == id.Value);
               
            if (flight == null)
            {
                return new NotFoundViewResult("FlightNotFound"); 
            }

            return View(flight);
        }



        // GET: Flights/Create
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            //TODO: Add Aircrafts
            var aircrafts = await _aircraftRepository.GetAircraftListAsync();
            var countries = await _countryRepository.GetCountryListAsync();
            var model = new FlightViewModel
            {
                Countries = countries.Select(country => new SelectListItem
                {
                    Value = country.Id.ToString(),
                    Text = country.Name
                }).ToList(),

                Aircrafts = aircrafts.Select(aircraft => new SelectListItem
                {
                    Value = aircraft.Id.ToString(),
                    Text = aircraft.Name
                }).ToList()

            };

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FlightViewModel model)
        {

            if (ModelState.IsValid)
            {
                var aircraft = await _aircraftRepository.GetByIdAsync(model.AircraftId);
                if (aircraft == null)
                {
                    ModelState.AddModelError(string.Empty, "Aircraft invalid. Please select a valid aircraft.");
                    return View(model);
                }

                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

                Random random = new Random();
                model.FlightNumber = random.Next(1000, 9999);

                model.Aircraft = aircraft;
                model.User = user;

                var flight = _converterHelper.ToFlight(model, true);

                await _flightRepository.CreateAsync(flight);
                flight.Aircraft = aircraft;

                await _ticketRepository.GenerateTicketsAsync(flight);

                return RedirectToAction(nameof(Index));
            }


            var countries = await _countryRepository.GetCountryListAsync();
            model.Countries = countries.Select(country => new SelectListItem
            {
                Value = country.Id.ToString(),
                Text = country.Name
            }).ToList()
            .OrderBy(c => c.Text);


            var aircrafts = await _aircraftRepository.GetAircraftListAsync();
            model.Aircrafts = aircrafts.Select(aircraft => new SelectListItem
            {
                Value = aircraft.Id.ToString(),
                Text = aircraft.Name
            }).ToList()
            .OrderBy(a => a.Text);


            return View(model);
        }

        // GET: Flights/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("FlightNotFound");
            }

            var flight = await _flightRepository.GetByIdAsync(id.Value);
            if (flight == null)
            {
                return new NotFoundViewResult("FlightNotFound");
            }
            var model = _converterHelper.ToFlightViewModel(flight);

            var countries = await _countryRepository.GetCountryListAsync();
            model.Countries = countries.Select(country => new SelectListItem
            {
                Value = country.Id.ToString(),
                Text = country.Name
            });

            var aircrafts = await _aircraftRepository.GetAircraftListAsync();
            model.Aircrafts = aircrafts.Select(aircraft => new SelectListItem
            {
                Value = aircraft.Id.ToString(),
                Text = aircraft.Name
            });

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(FlightViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingFlight = await _flightRepository.GetByIdAsync(model.Id);
                    if (existingFlight == null)
                    {
                        return new NotFoundViewResult("FlightNotFound");
                    }

                    bool isAircraftChanged = existingFlight.AircraftId != model.AircraftId;

                    existingFlight.OriginCountryId = model.OriginCountryId;
                    existingFlight.DestinationCountryId = model.DestinationCountryId;
                    existingFlight.Date = model.Date;
                    existingFlight.AircraftId = model.AircraftId;
                    model.FlightNumber = existingFlight.FlightNumber;

                    await _flightRepository.UpdateAsync(existingFlight);

                    if (isAircraftChanged)
                    {
                        var aircraft = await _aircraftRepository.GetByIdAsync(model.AircraftId);
                        if (aircraft == null)
                        {
                            ModelState.AddModelError(string.Empty, "Invalid aircraft. Please select a valid aircraft.");
                            return View(model);
                        }

                        existingFlight.Aircraft = aircraft;

                        await _ticketRepository.UpdateTicketsFromFlightAsync(existingFlight);
                    }


                }
                catch (DbUpdateConcurrencyException)
                {

                        return new NotFoundViewResult("FlightNotFound");

                }

                return RedirectToAction(nameof(Index));
            }

            var countries = await _countryRepository.GetCountryListAsync();
            model.Countries = countries.Select(country => new SelectListItem
            {
                Value = country.Id.ToString(),
                Text = country.Name
            }).ToList();

            var aircrafts = await _aircraftRepository.GetAircraftListAsync();
            model.Aircrafts = aircrafts.Select(aircraft => new SelectListItem
            {
                Value = aircraft.Id.ToString(),
                Text = aircraft.Name
            }).ToList();

            return View(model);
        }

        // GET: Flights/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("FlightNotFound");
            }

            var flights = await _flightRepository.GetAllFlightsWithCountriesAsync();
            var flight = flights.FirstOrDefault(f => f.Id == id.Value);
            if (flight == null)
            {
                return new NotFoundViewResult("FlightNotFound");
            }

            return View(flight);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var flight = await _flightRepository.GetByIdAsync(id);

            if (flight == null)
            {
                return new NotFoundViewResult("FlightNotFound");
            }

            await _ticketRepository.RemoveTicketsFromFlightAsync(flight);

            await _flightRepository.DeleteAsync(flight);

            return RedirectToAction(nameof(Index));
        }


    }
}
